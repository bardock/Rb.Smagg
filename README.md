# Rb.Smagg

## Setup

### NGINX

1. https://www.nginx.com/blog/setting-up-nginx/ (up to "Opening Your Web Page")
1. https://github.com/wandenberg/nginx-push-stream-module#installation (gcc and other libs might be required)
1. Run: ``sudo /usr/local/nginx/sbin/nginx -c /home/ubuntu/nginx-push-stream-module/misc/nginx.conf``
1. Set url into ``App.config``

### Twitter App

1. Creat an app: https://apps.twitter.com/
1. Go to "Keys and Access Tokens"
1. "Regenerate My Access Token and Token Secret"
1. Set credentials into ``App.config``