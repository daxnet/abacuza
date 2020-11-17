#!/bin/bash
export SolutionDir=$(PWD)/../..
dotnet publish -c Release -f net5.0 -r linux-x64 -o $(PWD)/../../published/Abacuza.JobRunners.Spark
