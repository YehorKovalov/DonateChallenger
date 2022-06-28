import Oidc, { UserManagerSettings } from 'oidc-client'

export const oidcConfiguration: UserManagerSettings = {
     authority: `${process.env.REACT_APP_IDENTITY_URL}`,
     client_id: `${process.env.REACT_APP_CLIENT_ID}`,
     redirect_uri: `${process.env.REACT_APP_REDIRECT_URL}`,
     scope: `${process.env.REACT_APP_SCOPE}`,
     response_type: `${process.env.REACT_APP_RESPONSE_TYPE}`,
     post_logout_redirect_uri: `${process.env.REACT_APP_POST_LOGOUT_REDIRECT_URL}`,
     automaticSilentRenew: true,
     client_secret: `${process.env.REACT_APP_CLIENT_SECRET}`,
     userStore: new Oidc.WebStorageStateStore ({ store: window.sessionStorage }),
     silent_redirect_uri: `${process.env.REACT_APP_SILENT_REDIRECT_URL}`
}