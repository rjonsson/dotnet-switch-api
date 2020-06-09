# dotnet-switch-api

Start LocalDB with `SqlLocalDB create WebApi`



Endpoint: [POST] /api/NetworkSwitches
```
{
  "name": "se1494sw01",
  "domain": "domain.net",
  "ip": "192.168.1.1",
  "status": 2,
  "ports": [
    {
      "name": "GigabitEthernet0/1",
      "mode": 0,
      "shutdown": false,
      "vlans": [
        {
          "vlan": {
            "id": 5,
            "name": "Vlan 5 Name "
          }
        }
      ]
    }
  ]
}
```

Endpoint: [GET] /api/NetworkSwitches/{id}
Endpoint: [GET] /api/NetworkSwitches