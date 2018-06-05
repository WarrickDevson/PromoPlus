let backendHost;

const hostname = window && window.location && window.location.hostname;

const apiVersion = 'v1.0';

if(hostname === 'localhost') {
  backendHost = 'http://localhost:61250';
} else { 
    backendHost = 'http://promoterplusserverless-prod.eu-west-1.elasticbeanstalk.com';
}

export const API_ROOT = `${backendHost}/api/`+apiVersion;