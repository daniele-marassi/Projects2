Imports Windows.ApplicationModel.AppService
Imports Windows.ApplicationModel.Background
Imports Windows.ApplicationModel.VoiceCommands

Public NotInheritable Class FooService
	Implements IBackgroundTask

	Private serviceDeferral As BackgroundTaskDeferral
	Private voiceServiceConnection As VoiceCommandServiceConnection

	Public Async Sub Run(taskInstance As IBackgroundTaskInstance) Implements IBackgroundTask.Run
		'Take a service deferral so the service isn't terminated
		serviceDeferral = taskInstance.GetDeferral()
		AddHandler taskInstance.Canceled, AddressOf OnTaskCanceled

		Dim triggerDetails = TryCast(taskInstance.TriggerDetails, AppServiceTriggerDetails)

		If triggerDetails IsNot Nothing AndAlso triggerDetails.Name = "FooService" Then
			Try
				voiceServiceConnection = VoiceCommandServiceConnection.FromAppServiceTriggerDetails(triggerDetails)

				AddHandler voiceServiceConnection.VoiceCommandCompleted, AddressOf VoiceCommandCompleted

				Dim voiceCommand As VoiceCommand = Await voiceServiceConnection.GetVoiceCommandAsync()

				Select Case voiceCommand.CommandName
					Case "fooServiceCommand"
						Dim first = voiceCommand.Properties("first")(0)
						Dim last = voiceCommand.Properties("last")(0)
						SendCompletionMessageForPhone(first, last)
						Exit Select

					' As a last resort launch the app in the foreground
					Case Else
						LaunchAppInForeground()
				End Select
			Catch ex As Exception

			Finally
				If serviceDeferral IsNot Nothing Then
					'Complete the service deferral
					serviceDeferral.Complete()
				End If
			End Try
		End If
	End Sub
	Private Sub OnTaskCanceled(sender As IBackgroundTaskInstance, reason As BackgroundTaskCancellationReason)
		serviceDeferral.Complete()
	End Sub
	Private Sub VoiceCommandCompleted(ByVal sender As VoiceCommandServiceConnection, ByVal args As VoiceCommandCompletedEventArgs)
		If serviceDeferral IsNot Nothing Then
			'Complete the service deferral
			serviceDeferral.Complete()
		End If
	End Sub
	Private Async Sub SendCompletionMessageForPhone(ByVal first As String, ByVal last As String)
		' First, create the VoiceCommandUserMessage with the strings 
		' that Cortana will show and speak.
		Dim userMessage = New VoiceCommandUserMessage()
		userMessage.DisplayMessage = String.Format("{0} {1}'s due is {2}", first, last, "$12.50")
		userMessage.SpokenMessage = String.Format("{0}'s due is {1}", first, "$12.50")

		' Optionally, present visual information about the answer.
		' For this example, create a List(Of VoiceCommandContentTile)

		' Create the VoiceCommandResponse from the userMessage and list    
		' of content tiles.
		Dim response = VoiceCommandResponse.CreateResponse(userMessage)

		' Cortana will present a "Go to app_name" link that the user 
		' can tap to launch the app. 
		' Pass in a launch to enable the app to deep link to a page 
		' relevant to the voice command.
		response.AppLaunchArgument = String.Format("first={0},last={1}", first, last)

		' Ask Cortana to display the user message and content tile and 
		' also speak the user message.
		Await voiceServiceConnection.ReportSuccessAsync(response)
	End Sub
	Private Async Sub LaunchAppInForeground()
		Dim userMessage = New VoiceCommandUserMessage()
		userMessage.SpokenMessage = "Launching Foo"

		Dim response = VoiceCommandResponse.CreateResponse(userMessage)

		' When launching the app in the foreground, pass an app 
		' specific launch parameter to indicate what page to show.
		response.AppLaunchArgument = "showFoo=true"

		Await voiceServiceConnection.RequestAppLaunchAsync(response)
	End Sub
End Class
