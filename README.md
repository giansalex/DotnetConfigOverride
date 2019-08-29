# Config Override
[![Build status](https://ci.appveyor.com/api/projects/status/3pyfy8kt2dqeihwa?svg=true)](https://ci.appveyor.com/project/giansalex/dotnetconfigoverride) ![Nuget](https://img.shields.io/nuget/v/DotnetConfigOverride?style=flat-square)    

Allow teams to override Web.config

## Install
```
Install-Package DotnetConfigOverride
```

## Usage
In `Global.asax.cs`

```csharp
DotnetConfig.WebConfig.Override();
```

Directory Tree (no include Web.Override.config in project)

![Directory Tree](https://raw.githubusercontent.com/giansalex/DotnetConfigOverride/master/assets/tree.png)

Add to `.gitignore`
```
Web.Override.config
```

In this moment, only `appSettings` and `connectionStrings` are supported.
```xml
<?xml version="1.0" encoding="UTF-8"?>
<configuration>
  <appSettings>
    <add key="Team" value="ABC"/>
  </appSettings>
  <connectionStrings>
    <add name="Default" connectionString="Data Source=.;Initial Catalog=DB_ABC;" providerName="System.Data.SqlClient" />
  </connectionStrings>
</configuration>
```
