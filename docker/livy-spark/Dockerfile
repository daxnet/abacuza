FROM openjdk:8

# Setting version and environment variables
ENV SPARK_HOME=/spark
ENV SPARK_VERSION=3.1.2
ENV HADOOP_VERSION=3.3.1
ENV DOTNET_SPARK_WROKER_VERSION=2.0.0
ENV AWS_JAVA_SDK_VERSION=1.11.871
ENV LIVY_VERSION=0.7.1
ENV SPARK_DIST_CLASSPATH=/hadoop-conf/:/hadoop/share/hadoop/common/lib/*:/hadoop/share/hadoop/common/*:/hadoop/share/hadoop/hdfs:/hadoop/share/hadoop/hdfs/lib/*:/hadoop/share/hadoop/hdfs/*:/hadoop/share/hadoop/yarn:/hadoop/share/hadoop/yarn/lib/*:/hadoop/share/hadoop/yarn/*:/hadoop/share/hadoop/mapreduce/lib/*:/hadoop/share/hadoop/mapreduce/*

# Getting .NET Core 3.1
RUN wget https://packages.microsoft.com/config/debian/10/packages-microsoft-prod.deb -O packages-microsoft-prod.deb && \
    dpkg -i packages-microsoft-prod.deb && \
    apt-get update; \
    apt-get install -y apt-transport-https && \
    apt-get update && \
    apt-get install -y dotnet-sdk-3.1 && \
    rm -f packages-microsoft-prod.deb

# Getting Microsoft.Spark.Worker
RUN wget https://daxnetarchivestorage.blob.core.windows.net/artifacts/Microsoft.Spark.Worker.netcoreapp3.1.linux-x64-$DOTNET_SPARK_WROKER_VERSION.tar.gz && \
    tar -zxf Microsoft.Spark.Worker.netcoreapp3.1.linux-x64-$DOTNET_SPARK_WROKER_VERSION.tar.gz && \
    mv Microsoft.Spark.Worker-$DOTNET_SPARK_WROKER_VERSION Microsoft.Spark.Worker && \
    rm -f Microsoft.Spark.Worker.netcoreapp3.1.linux-x64-$DOTNET_SPARK_WROKER_VERSION.tar.gz

ENV DOTNET_WORKER_DIR=/Microsoft.Spark.Worker

# Getting Spark
RUN wget https://daxnetarchivestorage.blob.core.windows.net/artifacts/spark-$SPARK_VERSION-bin-without-hadoop.tgz && \
    tar -xzf spark-$SPARK_VERSION-bin-without-hadoop.tgz && \
    mv spark-$SPARK_VERSION-bin-without-hadoop spark && \
    rm -f spark-$SPARK_VERSION-bin-without-hadoop.tgz

# Getting Hadoop
RUN wget https://daxnetarchivestorage.blob.core.windows.net/artifacts/hadoop-$HADOOP_VERSION.tar.gz && \
    tar -xzf hadoop-$HADOOP_VERSION.tar.gz && \
    mv hadoop-$HADOOP_VERSION hadoop && \
    rm -f hadoop-$HADOOP_VERSION.tar.gz

# Getting hadoop-aws jars
RUN cd /spark/jars && \
    wget https://daxnetarchivestorage.blob.core.windows.net/artifacts/hadoop-aws-$HADOOP_VERSION.jar && \
    wget https://daxnetarchivestorage.blob.core.windows.net/artifacts/aws-java-sdk-bundle-$AWS_JAVA_SDK_VERSION.jar

# Getting Livy
RUN wget https://daxnetarchivestorage.blob.core.windows.net/artifacts/apache-livy-$LIVY_VERSION-incubating-bin.zip && \
    unzip apache-livy-$LIVY_VERSION-incubating-bin.zip && \
    mv apache-livy-$LIVY_VERSION-incubating-bin livy && \
    rm -f apache-livy-$LIVY_VERSION-incubating-bin.zip
RUN cd /livy/jars && \
    cp /spark/jars/hadoop-aws-$HADOOP_VERSION.jar .
RUN mkdir -p /livy/logs/ && cp /livy/conf/log4j.properties.template /livy/conf/log4j.properties

# Configuring Hadoop and Livy
WORKDIR /livy
ENV SPARK_CONF_DIR=/spark/conf
ENV HADOOP_CONF_DIR=/hadoop/etc/hadoop
COPY hdfs-site.tmpl /hadoop/etc/hadoop/
COPY livy.conf.tmpl /livy/conf/
COPY log4j.properties /spark/conf/log4j.properties

# Dockerize the parameters from env variables
ENV DOCKERIZE_VERSION v0.6.1
RUN wget https://daxnetarchivestorage.blob.core.windows.net/artifacts/dockerize-linux-amd64-$DOCKERIZE_VERSION.tar.gz \
    && tar -C /usr/local/bin -xzvf dockerize-linux-amd64-$DOCKERIZE_VERSION.tar.gz \
    && rm dockerize-linux-amd64-$DOCKERIZE_VERSION.tar.gz

CMD dockerize -template /hadoop/etc/hadoop/hdfs-site.tmpl:/hadoop/etc/hadoop/hdfs-site.xml dockerize -template /livy/conf/livy.conf.tmpl:/livy/conf/livy.conf ./bin/livy-server
