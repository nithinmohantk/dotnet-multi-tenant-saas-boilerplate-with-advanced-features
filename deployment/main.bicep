@description('The name of the environment (e.g. dev, prod)')
param environmentName string = 'dev'

@description('The location for all resources.')
param location string = resourceGroup().location

@description('The name of the SQL Server administrator.')
param sqlAdminLogin string = 'saasadmin'

@description('The password for the SQL Server administrator.')
@secure()
param sqlAdminLoginPassword string

var appName = 'saas-boilerplate-${environmentName}'
var sqlServerName = 'sql-${appName}'
var appServicePlanName = 'plan-${appName}'
var webAppName = 'app-${appName}'
var redisName = 'redis-${appName}'
var appInsightsName = 'ai-${appName}'
var eventGridTopicName = 'evgt-${appName}'

// Application Insights
resource appInsights 'Microsoft.Insights/components@2020-02-02' = {
  name: appInsightsName
  location: location
  kind: 'web'
  properties: {
    Application_Type: 'web'
  }
}

// App Service Plan
resource appServicePlan 'Microsoft.Web/serverfarms@2022-03-01' = {
  name: appServicePlanName
  location: location
  sku: {
    name: 'S1'
    tier: 'Standard'
  }
}

// Web App
resource webApp 'Microsoft.Web/sites@2022-03-01' = {
  name: webAppName
  location: location
  properties: {
    serverFarmId: appServicePlan.id
    siteConfig: {
      netFrameworkVersion: 'v8.0'
      appSettings: [
        {
          name: 'APPINSIGHTS_INSTRUMENTATIONKEY'
          value: appInsights.properties.InstrumentationKey
        }
        {
          name: 'APPLICATIONINSIGHTS_CONNECTION_STRING'
          value: appInsights.properties.ConnectionString
        }
        {
          name: 'ASPNETCORE_ENVIRONMENT'
          value: 'Production'
        }
      ]
    }
  }
}

// SQL Server
resource sqlServer 'Microsoft.Sql/servers@2022-05-01-preview' = {
  name: sqlServerName
  location: location
  properties: {
    administratorLogin: sqlAdminLogin
    administratorLoginPassword: sqlAdminLoginPassword
  }
}

// Elastic Pool
resource elasticPool 'Microsoft.Sql/servers/elasticPools@2022-05-01-preview' = {
  parent: sqlServer
  name: 'pool-${environmentName}'
  location: location
  sku: {
    name: 'StandardPool'
    tier: 'Standard'
    capacity: 50
  }
  properties: {
    perDatabaseSettings: {
      minCapacity: 0
      maxCapacity: 10
    }
  }
}

// Master Database (Shared)
resource masterDb 'Microsoft.Sql/servers/databases@2022-05-01-preview' = {
  parent: sqlServer
  name: 'SaasBoilerplateDb'
  location: location
  sku: {
    name: 'ElasticPool'
    tier: 'Standard'
    capacity: 0
  }
  properties: {
    elasticPoolId: elasticPool.id
  }
}

// Redis Cache
resource redis 'Microsoft.Cache/redis@2023-04-01' = {
  name: redisName
  location: location
  properties: {
    sku: {
      name: 'Basic'
      family: 'C'
      capacity: 0
    }
  }
}

// Event Grid Topic
resource eventGridTopic 'Microsoft.EventGrid/topics@2022-06-15' = {
  name: eventGridTopicName
  location: location
  properties: {
    inputSchema: 'EventGridSchema'
  }
}

// Front Door (Standard/Premium)
resource frontDoorProfile 'Microsoft.Cdn/profiles@2021-06-01' = {
  name: 'fd-${appName}'
  location: 'global'
  sku: {
    name: 'Standard_AzureFrontDoor'
  }
}

resource frontDoorEndpoint 'Microsoft.Cdn/profiles/afdEndpoints@2021-06-01' = {
  parent: frontDoorProfile
  name: 'fde-${appName}'
  location: 'global'
  properties: {
    enabledState: 'Enabled'
  }
}

resource frontDoorOriginGroup 'Microsoft.Cdn/profiles/originGroups@2021-06-01' = {
  parent: frontDoorProfile
  name: 'og-${appName}'
  properties: {
    loadBalancingSettings: {
      sampleSize: 4
      successfulSamplesRequired: 3
    }
    healthProbeSettings: {
      probePath: '/'
      probeRequestType: 'HEAD'
      probeProtocol: 'Http'
      probeIntervalInSeconds: 100
    }
  }
}

resource frontDoorOrigin 'Microsoft.Cdn/profiles/originGroups/origins@2021-06-01' = {
  parent: frontDoorOriginGroup
  name: 'origin-${appName}'
  properties: {
    hostName: webApp.properties.defaultHostName
    httpPort: 80
    httpsPort: 443
    originHostHeader: webApp.properties.defaultHostName
    priority: 1
    weight: 1000
  }
}

resource frontDoorRoute 'Microsoft.Cdn/profiles/afdEndpoints/routes@2021-06-01' = {
  parent: frontDoorEndpoint
  name: 'route-${appName}'
  properties: {
    originGroup: {
      id: frontDoorOriginGroup.id
    }
    supportedProtocols: [
      'Http'
      'Https'
    ]
    patternsToMatch: [
      '/*'
    ]
    forwardingProtocol: 'MatchRequest'
    linkToDefaultDomain: 'Enabled'
    httpsRedirect: 'Enabled'
  }
}

output webAppName string = webApp.name
output sqlServerName string = sqlServer.name
output redisHostName string = redis.properties.hostName
output frontDoorEndpointHostName string = frontDoorEndpoint.properties.hostName
