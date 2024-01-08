# KURULUM

- Dosyaları GitHub üzerinden indirin.
- Cmd ile proje dizinine gidin (örnek: `cd C:\Users\user\Desktop\PhoneBookMicroserviceTemplate-master`).
- `docker-compose up` komutunu çalıştırın.
- 'reportserviceconsumerbg' container ilk çalıştırıldığında kendi kendini bir defaya mahsus durduruyor. Docker'a gidip manuel olarak tekrar çalıştırdığınızda sorunsuz şekilde çalışıyor.
- RabbitMQ default portlarında çalıştırılıyor, ancak eğer sizin RabbitMQ'nuz farklı bir işleyişe sahip ise RabbitMqService içerisinde bulunan (`\src\Messaging\PhoneBookService.Messaging`) dosyada `connectionStr = "amqp://guest:guest@s_rabbitmq:5672/"` şeklindeki dizeyi değiştirmeniz yeterli, değiştirmeniz gerekirse kolay olsun diye burada direkt set ettim.
- ReportService.Api - swagger: [http://localhost:5050/swagger/index.html](http://localhost:5050/swagger/index.html)
- PhoneBookService.Api - swagger: [http://localhost:5000/swagger/index.html](http://localhost:5000/swagger/index.html)
- RabbitMQ Panel: [http://localhost:15672/#/](http://localhost:15672/#/)
- Adreslerinden servis endpoint'lerine ulaşabilirsiniz.

Hepsi bu kadar! Umarım beğenirsiniz 😄

<br />
<br />
<br />
<br />
<br />

# INSTALLATION

- Download the files from GitHub.
- Go to the project directory in Cmd (example: `cd C:\Users\user\Desktop\PhoneBookMicroserviceTemplate-master`).
- Run the command `docker-compose up`.
- The 'reportserviceconsumerbg' container stops itself once on the first run. If you go to Docker and manually restart it, it runs smoothly afterwards.
- RabbitMQ is running on default ports, but if your RabbitMQ operates differently, simply change the string in RabbitMqService (`\src\Messaging\PhoneBookService.Messaging`) file, `connectionStr = "amqp://guest:guest@s_rabbitmq:5672/"`, set here for your convenience if you need to change it.
- ReportService.Api - swagger: [http://localhost:5050/swagger/index.html](http://localhost:5050/swagger/index.html)
- PhoneBookService.Api - swagger: [http://localhost:5000/swagger/index.html](http://localhost:5000/swagger/index.html)
- RabbitMQ Panel: [http://localhost:15672/#/](http://localhost:15672/#/)
- You can access service endpoints from these addresses.

That's all! Hope you like it 😄
