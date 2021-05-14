''' <summary>
''' Provides application-specific behavior to supplement the default Application class.
''' </summary>
NotInheritable Class App
	Inherits Application

	''' <summary>
	''' Initializes a new instance of the App class.
	''' </summary>
	Public Sub New()
		Microsoft.ApplicationInsights.WindowsAppInitializer.InitializeAsync(
			Microsoft.ApplicationInsights.WindowsCollectors.Metadata Or
			Microsoft.ApplicationInsights.WindowsCollectors.Session)
		InitializeComponent()
	End Sub

	''' <summary>
	''' Invoked when the application is launched normally by the end user.  Other entry points
	''' will be used when the application is launched to open a specific file, to display
	''' search results, and so forth.
	''' </summary>
	''' <param name="e">Details about the launch request and process.</param>
	Protected Overrides Async Sub OnLaunched(e As Windows.ApplicationModel.Activation.LaunchActivatedEventArgs)
		'#If DEBUG Then
		'		' Show graphics profiling information while debugging.
		'		If System.Diagnostics.Debugger.IsAttached Then
		'			' Display the current frame rate counters
		'			Me.DebugSettings.EnableFrameRateCounter = True
		'		End If
		'#End If
		Dim storageFile = Await Windows.Storage.StorageFile.GetFileFromApplicationUriAsync(New Uri("ms-appx:///FooCommands.xml"))
		Await Windows.ApplicationModel.VoiceCommands.VoiceCommandDefinitionManager.InstallCommandDefinitionsFromStorageFileAsync(storageFile)

		Dim rootFrame As Frame = TryCast(Window.Current.Content, Frame)

		' Do not repeat app initialization when the Window already has content,
		' just ensure that the window is active

		If rootFrame Is Nothing Then
			' Create a Frame to act as the navigation context and navigate to the first page
			rootFrame = New Frame()

			AddHandler rootFrame.NavigationFailed, AddressOf OnNavigationFailed

			If e.PreviousExecutionState = ApplicationExecutionState.Terminated Then
				' TODO: Load state from previously suspended application
			End If
			' Place the frame in the current Window
			Window.Current.Content = rootFrame
		End If
		If rootFrame.Content Is Nothing Then
			' When the navigation stack isn't restored navigate to the first page,
			' configuring the new page by passing required information as a navigation
			' parameter
			rootFrame.Navigate(GetType(MainPage), e.Arguments)
		End If

		' Ensure the current window is active
		Window.Current.Activate()
	End Sub

	''' <summary>
	''' Invoked when Navigation to a certain page fails
	''' </summary>
	''' <param name="sender">The Frame which failed navigation</param>
	''' <param name="e">Details about the navigation failure</param>
	Private Sub OnNavigationFailed(sender As Object, e As NavigationFailedEventArgs)
		Throw New Exception("Failed to load Page " + e.SourcePageType.FullName)
	End Sub

	''' <summary>
	''' Invoked when application execution is being suspended.  Application state is saved
	''' without knowing whether the application will be terminated or resumed with the contents
	''' of memory still intact.
	''' </summary>
	''' <param name="sender">The source of the suspend request.</param>
	''' <param name="e">Details about the suspend request.</param>
	Private Sub OnSuspending(sender As Object, e As SuspendingEventArgs) Handles Me.Suspending
		Dim deferral As SuspendingDeferral = e.SuspendingOperation.GetDeferral()
		' TODO: Save application state and stop any background activity
		deferral.Complete()
	End Sub

	Protected Overrides Async Sub OnActivated(ByVal e As IActivatedEventArgs)
		' Was the app activated by a voice command?
		If e.Kind <> Windows.ApplicationModel.Activation.ActivationKind.VoiceCommand Then
			Return
		End If
		Dim commandArgs = TryCast(e, Windows.ApplicationModel.Activation.VoiceCommandActivatedEventArgs)

		' Didn't really work for me... 
		'Dim speechRecognitionResult As Windows.ApplicationModel.VoiceCommands.VoiceCommand.SpeechRecognitionResult = commandArgs.Result
		Dim speechRecognitionResult As Windows.Media.SpeechRecognition.SpeechRecognitionResult = commandArgs.Result
		' Get the name of the voice command and the text spoken
		Dim voiceCommandName As String = speechRecognitionResult.RulePath(0)
		Dim textSpoken As String = speechRecognitionResult.Text
		' The commandMode is either "voice" or "text", and it indicates how the voice command was entered by the user.
		' Apps should respect "text" mode by providing feedback in a silent form.
		' Didn't work for me
		'Dim commandMode As String = SemanticInterpretation("commandMode", speechRecognitionResult)
		Dim commandMode As String = speechRecognitionResult.SemanticInterpretation.Properties("commandMode")(0)
		Dim rootFrame As Frame = TryCast(Window.Current.Content, Frame)
		' Root frame better not be nothing, we set it when launching th app...

		Select Case voiceCommandName
			Case "fooAppCommand"
				' Get the properties Cortana passed us
				Dim first As String = speechRecognitionResult.SemanticInterpretation.Properties("first")(0)
				Dim last As String = speechRecognitionResult.SemanticInterpretation.Properties("last")(0)
				' Create a navigation parameter string to pass to the page
				' Set the page where to navigate for this voice command
				'navigateToPageType = GetType(TripPage)
				Dim mainPage = TryCast(rootFrame.Content, MainPage)
				If mainPage IsNot Nothing Then
					mainPage.Response = String.Format("Mode = {0}.  {1} {2} due is {3}", commandMode, first, last, "$12.50")
				End If

			Case Else
				' There is no match for the voice command name. Navigate to MainPage
				'navigateToPageType = GetType(MainPage)
				Dim messageDialog = New Windows.UI.Popups.MessageDialog("Unknown command: " + voiceCommandName)
				Await messageDialog.ShowAsync()

		End Select

		'If Not rootFrame.Navigate(navigateToPageType, navigationParameterString) Then
		'	Throw New Exception("Failed to create voice command page")
		'End If
	End Sub

End Class
