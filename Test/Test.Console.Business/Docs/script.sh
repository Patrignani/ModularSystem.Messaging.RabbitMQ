if [ -d "../publish" ]; then
sudo rm -r ~/publish;
sudo mkdir ~/publish;
else
sudo mkdir ~/publish;
fi

sudo dotnet publish ~/git/messaging.rabbitmq/Test/Test.Console.Business/ -c Release -f netcoreapp3.1 -o ~/publish;

chmod 777 ~/publish/./Test.Console.Business;
~/publish/./Test.Console.Business;
