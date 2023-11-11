targetScope = 'resourceGroup'

param projectName string
param projectLocation string = resourceGroup().location

var projectAppServiceName = 'app-${length(projectName) > 56 ? substring(projectName, 0, 56) : projectName}'
var projectAppServicePlanName = 'asp-${length(projectName) > 36 ? substring(projectName, 0, 36) : projectName}'
var projectDocumentDBAccountName = 'cosmos-${uniqueString(resourceGroup().id)}'

resource projectDocumentDBAccount 'Microsoft.DocumentDB/databaseAccounts@2023-04-15' = {
  name: projectDocumentDBAccountName
  location: projectLocation
  properties: {
    enableFreeTier: true
    databaseAccountOfferType: 'Standard'
    consistencyPolicy: {
      defaultConsistencyLevel: 'Session'
    }
    locations: [
      {
        locationName: projectLocation
      }
    ]
  }
}

resource projectAppServicePlan 'Microsoft.Web/serverfarms@2022-09-01' = {
  name: projectAppServicePlanName
  location: projectLocation
  sku: {
    name: 'F1'
  }
  kind: 'linux'
  properties: {
    reserved: true
  }
}

resource projectAppService 'Microsoft.Web/sites@2022-09-01' = {
  name: projectAppServiceName
  location: projectLocation
  properties: {
    serverFarmId: projectAppServicePlan.id
    siteConfig: {
      linuxFxVersion: 'DOTNETCORE|6.0'
    }
  }
}
