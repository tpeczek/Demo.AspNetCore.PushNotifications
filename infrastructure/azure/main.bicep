targetScope = 'subscription'

param projectName string
param projectLocation string

resource resourceGroupResource 'Microsoft.Resources/resourceGroups@2022-09-01' = {
  name: 'rg-${projectName}'
  location: projectLocation
}

module resourceGroupModule 'resource-group.bicep' = {
  name: '${projectName}-rg'
  scope: resourceGroup(resourceGroupResource.name)
  params: {
    projectName: projectName
    projectLocation: resourceGroupResource.location
  }
}
