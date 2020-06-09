using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.EntityFrameworkCore;
using switch_api.DataAccess;
using switch_api.Models;

namespace switch_api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class NetworkSwitchesController : ControllerBase
    {
        private readonly NetworkSwitchContext _context;

        public NetworkSwitchesController(NetworkSwitchContext context)
        {
            _context = context;
        }

        // GET: api/NetworkSwitches
        [HttpGet]
        public async Task<ActionResult<IEnumerable<NetworkSwitch>>> GetNetworkSwitches()
        {


            return await _context.NetworkSwitches
                                        .Include(sw => sw.Ports)
                                            .ThenInclude(p => p.Vlans)
                                                .ThenInclude(pv => pv.Vlan)
                                        .ToListAsync();

        }

        // GET: api/NetworkSwitches/5
        [HttpGet("{id}")]
        public async Task<ActionResult<NetworkSwitch>> GetNetworkSwitch(int id)
        {
            //var networkSwitch = await _context.NetworkSwitches.FindAsync(id);

            var networkSwitch = await _context.NetworkSwitches
                                              .Include(swi => swi.Ports)
                                                  .ThenInclude(p => p.Vlans)
                                                      .ThenInclude(pv => pv.Vlan)
                                              .SingleOrDefaultAsync(sw => sw.Id == id);
            
            /*
            var cartIncludingItems = _context.
                
                .Include(cart => cart.Items).ThenInclude(row => row.Item).First(cart => cart.Id == 1);
            */


            if (networkSwitch == null)
            {
                return NotFound();
            }

            return networkSwitch;
        }

        // PUT: api/NetworkSwitches/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for
        // more details, see https://go.microsoft.com/fwlink/?linkid=2123754.
        [HttpPut("{id}")]
        public async Task<IActionResult> PutNetworkSwitch(int id, NetworkSwitch networkSwitch)
        {
            if (id != networkSwitch.Id)
            {
                return BadRequest();
            }

            _context.Entry(networkSwitch).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!NetworkSwitchExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return NoContent();
        }

        // POST: api/NetworkSwitches
        // To protect from overposting attacks, enable the specific properties you want to bind to, for
        // more details, see https://go.microsoft.com/fwlink/?linkid=2123754.
        [HttpPost]
        public async Task<ActionResult<NetworkSwitch>> PostNetworkSwitch(NetworkSwitch networkSwitch)
        {


            // Add all vlans from the switch to a list and check if it needs creating
            List<Vlan> vlans = new List<Vlan>();
            networkSwitch.Ports.ForEach(port =>
                        port.Vlans.ForEach(portvlan =>
                               vlans.Add(new Vlan
                               {
                                   Id = portvlan.Vlan.Id,
                                   Name = portvlan.Vlan.Name
                               })));

            // Compare with database list and add missing vlans to db
            var existingVlans = _context.Vlans.ToList();
            vlans.RemoveAll(vlan => existingVlans.Exists(existing => existing.Id == vlan.Id));
            vlans.ForEach(vlan => _context.Vlans.Add(vlan));
            await _context.SaveChangesAsync();

            // Remove vlan-information and re-add tracked vlan-objects (in order)
            networkSwitch.Ports.ForEach(port =>
            {
                int[] vlannumbers = port.Vlans.Select(x => x.Vlan.Id).ToArray();
                port.Vlans.Clear();
                foreach(int number in vlannumbers)
                {
                    port.Vlans.Add(new PortVlan { Vlan = _context.Vlans.FirstOrDefault(vlan => vlan.Id == number )});
                }
            });
                


            _context.NetworkSwitches.Add(networkSwitch);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetNetworkSwitch), new { id = networkSwitch.Id }, networkSwitch);
            //return CreatedAtAction(nameof(GetNetworkSwitch), new { id = networkSwitch.Id }, networkSwitch);
        }

        // DELETE: api/NetworkSwitches/5
        [HttpDelete("{id}")]
        public async Task<ActionResult<NetworkSwitch>> DeleteNetworkSwitch(int id)
        {
            var networkSwitch = await _context.NetworkSwitches.FindAsync(id);
            
            if (networkSwitch == null)
            {
                return NotFound();
            }

            _context.NetworkSwitches.Remove(networkSwitch);
            await _context.SaveChangesAsync();

            return networkSwitch;
        }

        private bool NetworkSwitchExists(int id)
        {
            return _context.NetworkSwitches.Any(e => e.Id == id);
        }
    }
}
