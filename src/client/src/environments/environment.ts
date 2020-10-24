/**
 * @license
 * Copyright Akveo. All Rights Reserved.
 * Licensed under the MIT License. See License.txt in the project root for license information.
 */
// The file contents for the current environment will overwrite these during build.
// The build system defaults to the dev environment which uses `environment.ts`, but if you do
// `ng build --env=prod` then `environment.prod.ts` will be used instead.
// The list of which env maps to which file can be found in `.angular-cli.json`.

export const environment = {
  production: false,
  clusterServiceBaseUrl: 'http://localhost:9023/',
  jobServiceBaseUrl: 'http://localhost:9024/',
  commonServiceBaseUrl: 'http://localhost:9025/',
  endpointServiceBaseUrl: 'http://localhost:9026',
  projectServiceBaseUrl: 'http://localhost:9027',
};
