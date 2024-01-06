- KURULUM -

> Dosyaları Github üzerinden indirin. 
> Cmd ile proje dizinine gidin( örnek: cd C:\Users\user\Desktop\PhoneBookMicroserviceTemplate-master) 
> docker-compose up koutunu çalıştırın 
> 'reportserviceconsumerbg' container ilk çalıştırıldığında kendi kendini bir defaya mahsus durduruyor. Docker a gidip manuel olarak tekrar çalıştırdığınızda sorunsuz şekilde çalışıyor. 
> RabbitMQ default portlarında çalıştırılıyor, ancak eğer sizin RabbitMQ nuz farklı bir işleyişe sahip ise RabbitMqService içerisinde bulunan (\src\Messaging\PhoneBookService.Messaging) dosyada connectionStr = "amqp://guest:guest@s_rabbitmq:5672/" şeklindeki dizeyi değiştirmeniz yeterli, değiştirmeniz gerekirse kolay olsun diye burada direkt set ettim.
> Endpointler: 
> ReportService.Api - swagger : http://localhost:5050/swagger/index.html 
> PhoneBookService.Api - swagger : http://localhost:5000/swagger/index.html
> RabbitMQ Panel : http://localhost:15672/#/
> adreslerinden servis endpoint' lerine ulaşabilirsiniz. 
> 
> 
> Hepsi bu kadar :D umarım beğenirsiniz :D


- KURULUM -

> Download the files from Github.
> Go to the project directory using the command prompt (example: cd C:\Users\user\Desktop\PhoneBookMicroserviceTemplate-master)
> Run the command 'docker-compose up'
> On first launch, the 'reportserviceconsumerbg' container stops itself once. If you manually restart it in Docker, it will then work smoothly. 
> RabbitMQ is running on the default ports. However, if your RabbitMQ setup differs, you can change the connection string in the file located at (\src\Messaging\PhoneBookService.Messaging). The connection string "amqp://guest:guest@s_rabbitmq:5672/" can be modified as needed; I've set it here for your convenience if changes are necessary.
> Access service endpoints through these addresses:
> ReportService.Api - swagger: http://localhost:5050/swagger/index.html
> PhoneBookService.Api - swagger : http://localhost:5000/swagger/index.html
> RabbitMQ Panel : http://localhost:15672/#/
> 
> 
> That's all :D Hope you like it :D
