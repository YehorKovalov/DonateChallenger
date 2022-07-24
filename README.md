# GETTING STARTED

### 1)  Make sure you have [installed](https://www.docker.com/get-started/) Docker in your environment.

### 2) Open host file on your pc:
--------------------------------------------------------------------------------------------------------------------------------
WINDOWS OS:

  1) Navigate to c:\Windows\System32\Drivers\etc\hosts

  2) "Run as Administrator" with Notepad(for example).

  3) If you can’t view any of the listed directories you will need to configure Windows to show your hidden program or System Files

MAC OS:

  1) Press "command + space" -> input "Terminal"

  2) Execute sudo nano /etc/hosts.

  3) When you’re done, use Ctrl + O (followed by Enter) to save the file and then Ctrl + X to exit.
--------------------------------------------------------------------------------------------------------------------------------

Then path these lines

  127.0.0.1 donate-challenger.com
  
  0.0.0.0 donate-challenger.com
  
  192.168.0.2 donate-challenger.com


### 3) Execute step-by-step the next commands in your terminal:
      
      a) git clone https://github.com/YehorKovalov/DonateChallenger   (or clone it in another way)
      
      b) cd donatechallenger
      
      c) docker-compose up -d --build

### 4) Navigate to:   donate-challenger.com
