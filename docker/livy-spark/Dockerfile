FROM openjdk:8

# Setting version and environment variables
ENV SPARK_HOME=/spark
ENV SPARK_VERSION=2.4.5
ENV HADOOP_VERSION=3.1.4
ENV AWS_JAVA_SDK_VERSION=1.11.871
ENV LIVY_VERSION=0.7.0
ENV SPARK_DIST_CLASSPATH=/hadoop-conf/:/hadoop/share/hadoop/common/lib/*:/hadoop/share/hadoop/common/*:/hadoop/share/hadoop/hdfs:/hadoop/share/hadoop/hdfs/lib/*:/hadoop/share/hadoop/hdfs/*:/hadoop/share/hadoop/yarn:/hadoop/share/hadoop/yarn/lib/*:/hadoop/share/hadoop/yarn/*:/hadoop/share/hadoop/mapreduce/lib/*:/hadoop/share/hadoop/mapreduce/*

# Getting Spark
RUN wget https://archive.apache.org/dist/spark/spark-$SPARK_VERSION/spark-$SPARK_VERSION-bin-without-hadoop.tgz && \
    tar -xzf spark-$SPARK_VERSION-bin-without-hadoop.tgz && \
    mv spark-$SPARK_VERSION-bin-without-hadoop spark

# Getting Hadoop
RUN wget https://archive.apache.org/dist/hadoop/common/hadoop-$HADOOP_VERSION/hadoop-$HADOOP_VERSION.tar.gz && \
    tar -xzf hadoop-$HADOOP_VERSION.tar.gz && \
    mv hadoop-$HADOOP_VERSION hadoop

# Getting hadoop-aws jars
RUN cd /spark/jars && \
    wget https://repo.maven.apache.org/maven2/org/apache/hadoop/hadoop-aws/$HADOOP_VERSION/hadoop-aws-$HADOOP_VERSION.jar && \
    wget https://repo.maven.apache.org/maven2/com/amazonaws/aws-java-sdk-bundle/$AWS_JAVA_SDK_VERSION/aws-java-sdk-bundle-$AWS_JAVA_SDK_VERSION.jar

# Getting Livy
RUN wget http://archive.apache.org/dist/incubator/livy/$LIVY_VERSION-incubating/apache-livy-$LIVY_VERSION-incubating-bin.zip && \
    unzip apache-livy-$LIVY_VERSION-incubating-bin.zip && \
    mv apache-livy-$LIVY_VERSION-incubating-bin livy
RUN cd /livy/jars && \
    cp /spark/jars/hadoop-aws-$HADOOP_VERSION.jar .
RUN mkdir -p /livy/logs/ && cp /livy/conf/log4j.properties.template /livy/conf/log4j.properties

# Configuring Hadoop and Livy
WORKDIR /livy
ENV SPARK_CONF_DIR=/spark/conf
ENV HADOOP_CONF_DIR=/hadoop-conf/
COPY hdfs-site.tmpl /hadoop-conf/
COPY livy.conf.tmpl /livy/conf/
COPY log4j.properties /spark/conf/log4j.properties

# Dockerize the parameters from env variables
ENV DOCKERIZE_VERSION v0.6.1
RUN wget https://github.com/jwilder/dockerize/releases/download/$DOCKERIZE_VERSION/dockerize-linux-amd64-$DOCKERIZE_VERSION.tar.gz \
    && tar -C /usr/local/bin -xzvf dockerize-linux-amd64-$DOCKERIZE_VERSION.tar.gz \
    && rm dockerize-linux-amd64-$DOCKERIZE_VERSION.tar.gz

CMD dockerize -template /hadoop-conf/hdfs-site.tmpl:/hadoop-conf/hdfs-site.xml dockerize -template /livy/conf/livy.conf.tmpl:/livy/conf/livy.conf ./bin/livy-server
