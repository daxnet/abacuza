#!/bin/bash
mkdir -p published
dotnet publish -p:IncludeLicenseWhenGeneratePackage=false Abacuza.JobRunners.Spark.Sample/ -c Release -f net5.0 -r linux-x64 -o published/Abacuza.JobRunners.Spark.Sample
dotnet build -p:IncludeLicenseWhenGeneratePackage=false Abacuza.JobRunners.Spark.SDK.IO/ -c Release -r linux-x64 -o published/sdk_io
cp published/sdk_io/Abacuza.JobRunners.Spark.SDK.IO.dll published/Abacuza.JobRunners.Spark.Sample
rm -rf published/sdk_io
