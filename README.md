# Abacuza
A Simplified Data Processing Platform

[![Build Status](https://dev.azure.com/sunnycoding/Abacuza/_apis/build/status/Abacuza-Build-Pipeline?branchName=master)](https://dev.azure.com/sunnycoding/Abacuza/_build/latest?definitionId=8&branchName=master)
![Azure DevOps tests](https://img.shields.io/azure-devops/tests/sunnycoding/Abacuza/8)
![Docker Image Version (latest semver)](https://img.shields.io/docker/v/daxnet/abacuza-nginx?label=docker%20build%20version)

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

## How to Run (Debug Mode)
1. Follow the instructions in [How to Debug (Services)](#how-to-debug-services) to start the infrastructure services and the backend services
2. Go to the `src/client` directory
3. Run `npm install` to install the dependencies
4. Run `npm start` to start the Angular development server at localhost:4200
5. Navigate to http://localhost:4200 in a web browser to access the Abacuza Administrator dashboard

## How to Run
1. Execute the following command to run everything:
   
   `docker-compose up`

2. Navigate to http://localhost:9320 in a web browser to access the Abacuza Administrator dashboard

