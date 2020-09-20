# Abacuza
Simplified Data Processing Platform

## Prerequisites
- docker engine: v19.03 or above
- docker compose: v1.27.2 or above

## How to Build
1. Clone the repo:
   
   `git clone https://github.com/daxnet/abacuza`

2. Build everything with the following command:
   
   `docker-compose -f docker-compose.build.yaml build`

## How to Debug (Services)
1. Start the infrastructure services like database or redis cache:
   
   `docker-compose -f docker-compose.dev.yaml up`

2. Open `abacuza.sln` in Visual Studio 2019 from `src/services` directory
3. Press F5 to debug

## How to Run
1. It is preferrable to build the docker images before running Abacuza locally. Refer to [How to Build](#how-to-build)
2. Execute the following command to run everything:
   
   `docker-compose up`


