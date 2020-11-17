#!/bin/bash
export SolutionDir=$(PWD)/../../
mkdir -p published
cd Abacuza.JobRunners.Spark
dotnet publish -c Release -f net5.0 -r linux-x64 -o ../published/Abacuza.JobRunners.Spark
