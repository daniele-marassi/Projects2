import speech_recognition as sr
import datetime
import ctypes

ctypes.windll.user32.ShowWindow( ctypes.windll.kernel32.GetConsoleWindow(), 0 )

while True:
    recognizer_instance = sr.Recognizer()

    with sr.Microphone() as source:
        recognizer_instance.adjust_for_ambient_noise(source)
        recognizer_instance.pause_threshold = 1.0
        audio = recognizer_instance.listen(source)
    try:
        text = recognizer_instance.recognize_google(audio, language="it-IT")
        with open("SpeechRecognitionOutput.txt", "a") as text_file:
            text_file.write('{date:"'+datetime.datetime.now().strftime("%Y-%m-%dT%H:%M:%S.%fZ") + '", phrase:"'+ text+'"}' + "\n")
    except Exception as e:
        print(e)

#python SpeechRecognition.py @ execute
#pyinstaller --onefile SpeechRecognition.pyw #build
