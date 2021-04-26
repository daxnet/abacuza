#! /bin/bash
curl -v -X POST https://localhost:9051/api/account/register-role -d '{"name":"admin","description":"Administrator"}' -H 'Content-Type:application/json' --insecure
curl -v -X POST https://localhost:9051/api/account/register-user -d '{"userName":"super","password":"P@ssw0rd","displayName":"Super Admin","email":"super@abacuza.com","roleNames":["admin"]}' -H 'Content-Type:application/json' --insecure
curl -v -X POST https://localhost:9051/api/account/register-user -d '{"userName":"abacuza","password":"P@ssw0rd","displayName":"Abacuza User","email":"abacuza@abacuza.com"}' -H 'Content-Type:application/json' --insecure
