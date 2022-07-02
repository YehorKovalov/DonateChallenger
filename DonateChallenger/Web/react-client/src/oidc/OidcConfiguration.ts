import Oidc, { UserManagerSettings } from 'oidc-client'

export const oidcConfiguration: UserManagerSettings = {
     authority: `${process.env.REACT_APP_AUTHORITY}`,
     client_id: `${process.env.REACT_APP_CLIENT_ID}`,
     redirect_uri: `${process.env.REACT_APP_REDIRECT_URL}`,
     scope: `${process.env.REACT_APP_SCOPE}`,
     response_type: `${process.env.REACT_APP_RESPONSE_TYPE}`,
     post_logout_redirect_uri: `${process.env.REACT_APP_POST_LOGOUT_REDIRECT_URL}`,
     silent_redirect_uri: `${process.env.REACT_APP_POST_LOGOUT_REDIRECT_URL}`,
     client_secret: `${process.env.REACT_APP_SILENT_REDIRECT_URL}`,
     userStore: new Oidc.WebStorageStateStore ({ store: window.sessionStorage }),
     automaticSilentRenew: true,
     filterProtocolClaims: true,
     loadUserInfo: true,
     monitorSession: true
}