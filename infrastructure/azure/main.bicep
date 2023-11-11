targetScope = 'subscription'

param projectName string
param projectLocation string

@secure()
param pushServiceClientSubject string
@secure()
param pushServiceClientPublicKey string
@secure()
param pushServiceClientPrivateKey string

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
    pushServiceClientSubject: pushServiceClientSubject
    pushServiceClientPublicKey: pushServiceClientPublicKey
    pushServiceClientPrivateKey: pushServiceClientPrivateKey
  }
}
