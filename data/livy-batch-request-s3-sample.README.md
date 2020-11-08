## Basic Steps

.NET for Spark project must be built with Release and self-contained.

dotnet publish -c Release -f netcoreapp3.1 -r linux-x64

Then zip the published folder, name it to abacuza.jobrunners.spark.zip.

Then upload it to S3.

## JSON Sample Explained

Following is an example of the payload:

```json
// POST http://localhost:8998/batches
{
    "file": "s3a://data/jar/microsoft-spark-3-0_2.12-1.0.0.jar",
    "className": "org.apache.spark.deploy.dotnet.DotnetRunner",
    "args": [
        "s3a://data/jar/abacuza.jobrunners.spark.zip",
        "Abacuza.JobRunners.Spark",
        "a=1",
        "b=2"
    ]
}
```
- `file` is the name of the spark job runner, it should be `microsoft-spark-<version>.jar`
- `className` is the name of the class entry, it should be `org.apache.spark.deploy.dotnet.DotnetRunner`
- `args` contains the arguments
  - First argument is the zip file name of the self-contained application that we created in the [Basic Steps](#basic-steps) above
  - Second argument is the name of the executable application that contained in the zip file
  - Custom arguments can be added as the 3rd argument and so on

