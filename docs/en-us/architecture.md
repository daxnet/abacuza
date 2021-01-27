# Architecture
Following diagram shows the architecture of Abacuza.

![Architecture](../images/abacuza_architecture_diagram.png)

A typical data processing workflow is as follows:
1. **Data Analyst** uses Abacuza Administrator web portal to define and manage data processing clusters
2. **Data Analyst** defines _Job Runners_ for the job execution. A job runner determines on which type of cluster the job should be executed, and the template of a payload that will be submitted to the cluster when a job starts
3. **Data Analyst** creates a data processing _project_ in which the source of the raw data will be defined. Each project will have a _Job Runner_ attached, which identifies how the raw data would be submitted for processing. Moreover, a project also specifies the output location where the processed data should be stored
4. Within the project, **Data Analyst** chooses _input endpoint_ which defines the source of the raw data. Different input endpoints will have different configuration settings. The project details page will show the configuration settings for the selected input endpoint under the `Input` tab
5. Within the project, **Data Analyst** chooses _output endpoint_ which defines where the processed data should be stored. Different output endpoints will have different configuration settings. The project details page will show the configuration settings for the selected output endpoint under the `Output` tab
6. Within the project, **Data Analyst** specifies the _job runner_ to be used by the project
7. Within the project, **Data Analyst** clicks the `Submit` button to submit the data processing job. Under the `Revisions` tab, **Data Analyst** can check the data processing status and logs. Once the processing succeeded, the processed data will be stored to the location defined by _output endpoint_
8. Ideally, **System Administrators** could define Crond-based task scheduling mechanism which could execute the job submissions periodically

Generally:
- _Clusters_ describe WHERE the data should be processed
- _Job Runners_ describe HOW the data should be processed
- _Input Endpoints_ describe WHAT data should be processed
- _Output Endpoints_ describe the location of the processed data
- _Revisions_ maintain the status of each data processing job
- _Projects_ orchestrate data processing tasks and provide data processing console to end users

