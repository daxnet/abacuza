export const environment = {
  production: true,
  apiBaseUrl: '{{ .Env.API_BASE_URL }}',
  returnUrl: '{{ .Env.RETURN_URL }}',
  idpAuthority: '{{ .Env.IDP_AUTHORITY }}',
  idpRedirectUrl: '{{ .Env.IDP_REDIRECT_URL }}',
  idpClientId: 'web'
};
