for i in {1..40}
do
	cd ../../../../
	dotnet run Program.cs >> Experiment/ExampleNetwork/Result/Isolation/exampleNetworkVerifyTime.txt
done
