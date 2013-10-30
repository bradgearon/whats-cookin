For those who forget what why they bought all the things.
Meal planning per configured interval.

Paste a url (allrecipies or whatever) - uses embed.ly to parse metadata, and load images.
Responsive almost to 300px wide (for those who get hungry and want to know whats cookin')

```bash
npm install && bower install 
```
to run locally with auto reload (point your front-end to  /client/web/app):
```bash
grunt server
```
to build (point your front-end to /client/web/dist):
```bash
grunt build
```

client/
Yeoman
Node / Grunt / bower for client builds / deps
Bootstrap 3
AngularJs
Various angular modules (angular-bootstrap / ui)

server/
ServiceStack (nipples)
ServiceStack.Ormlite (ORM) / Funq (IOC)
Sqlite (storing the things)

to run:
spawn the server in iis / or mono
from /client/web in your favorite terminal:

Note:  
This was a quick project to replace a previous version.  The code is not perfect.
	