for i in {1..40}
do
	dotnet run Program.cs >> Experiment/ExampleNetwork/Result/Isolation/ModelBuildTime+VerificationTime.txt
done
