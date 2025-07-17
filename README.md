# Demo.AspNetCore.PushNotifications

Sample project for demonstrating Push Notifications based on Push API and Web Push Protocol in ASP.NET Core:

- [Push API](https://www.tpeczek.com/2017/12/push-notifications-and-aspnet-core-part.html)
- [Requesting Delivery](https://www.tpeczek.com/2018/01/push-notifications-and-aspnet-core-part.html) ([Lib.Net.Http.WebPush](https://github.com/tpeczek/Lib.Net.Http.WebPush))
- VAPID tokens caching
- [Replacing Messages & Urgency](https://www.tpeczek.com/2018/01/push-notifications-and-aspnet-core-part_18.html)
- [Special Cases](https://www.tpeczek.com/2019/02/push-notifications-and-aspnet-core-part.html)

## Running the Project

In order to run the project, some configuration is required. Inside *appsettings.json* there are placeholders to provide public and private VAPID keys:

```json
{
  "ConnectionStrings": {
    "PushSubscriptionSqliteDatabase": "Filename=./../pushsubscription.db"
  },
  "PushServiceClient": {
    "Subject": "https://localhost:65506/",
    "PublicKey": "<Application Server Public Key>",
    "PrivateKey": "<Application Server Private Key>"
  }
}
```

Those keys can be acquired with help of online generators (https://vapidkeys.com/, https://www.attheminute.com/vapid-key-generator).

## Consulting and Professional Services

Do you need help with any of my libraries, or with building features on top of what they provide? What about ASP.NET Core architecture, Azure architecture, or DevOps? I offer consulting and development services.

[![Book an Appointment](https://img.shields.io/badge/%20-Book%20an%20Appointment-%23006BFF?logo=calendly&logoColor=white&style=for-the-badge)](https://calendly.com/tpeczek/30min)
[![Send an Email](https://img.shields.io/badge/%20-Send%20an%20email-%23EA4335?logo=gmail&logoColor=white&style=for-the-badge)](mailto:tpeczek@gmail.com)

## Donating

My blog and open source projects are result of my passion for software development, but they require a fair amount of my personal time. If you got value from any of the content I create, then I would appreciate your support by [sponsoring me](https://github.com/sponsors/tpeczek) (either monthly or one-time).

## Copyright and License

Copyright © 2017 - 2025 Tomasz Pęczek

Licensed under the [MIT License](https://github.com/tpeczek/Demo.AspNetCore.PushNotifications/blob/master/LICENSE.md)
