# Contributing

Just some notes to get you up and running

## NSwag / Create API client

I used https://github.com/RicoSuter/NSwag/wiki/NSwagStudio to generate the client. 
You need to have a local ActivityWatch running on http://localhost:5600/api/swagger.json. 
The config I used is saved as ./API/V1/GeneratorConfig.nswag

The ./API/V1/Generated.cs needs a little fix ...
Some Method names start with a zero. Luckily you could just replace " 0" with just " " (without quotes). 

