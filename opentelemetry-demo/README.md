# Open Telemetry Demo

## Tech Stack

- [x] Open Telemetry
- ![alt text](doc/opentelemetry-demo.PNG)
- [x] Prometheus
- ![alt text](doc/prometheus-demo.PNG)
- [x] Grafana
- ![alt text](doc/grafana-demo.PNG)
- [x] Jaeger
- ![alt text](doc/greeting-demo.PNG)
- ![alt text](doc/jaeger-demo.PNG)

```c#
$ dotnet new web

$ dotnet build

$ dotnet run

$ curl 'http://localhost:5212/rolldice'

$ dotnet add package OpenTelemetry.Extensions

$ curl http://localhost:5212/

$ curl http://localhost:5212/metrics

# metrics
# prometheus
https://prometheus.io/download/

$ prometheus.exe
# http://localhost:9090

# grafana
# Download and install the OSS version of Grafana
https://grafana.com/grafana/download/10.0.0?pg=oss-graf&plcmt=hero-btn-1&platform=windows

# http://localhost:3000
the default username and password are both admin

$ dotnet run --roll-forward LatestMajor

# Jaeger (use 7zip to unzip tar.gz)
# https://www.jaegertracing.io/download/

$ docker pull jaegertracing/all-in-one:1.56

$ docker run --rm --name jaeger -e COLLECTOR_ZIPKIN_HOST_PORT=:9411 -p 6831:6831/udp -p 6832:6832/udp -p 5778:5778 -p 16686:16686 -p 4317:4317 -p 4318:4318 -p 14250:14250 -p 14268:14268 -p 14269:14269 -p 9411:9411 jaegertracing/all-in-one:1.56

$ docker run -d -p 4317:4317 -p 16686:16686 jaegertracing/all-in-one:1.56

$ docker ps

$ docker kill

# log shows
{"level":"info","ts":1697018618.767567,"caller":"grpc@v1.58.2/clientconn.go:2001","msg":"[core][Channel #1] Channel authority set to \"localhost:4317\"","system":"grpc","grpc_log":true}

# update appsetting.json OTLP_ENDPOINT_URL port=4317

# http://localhost:16686/

```
