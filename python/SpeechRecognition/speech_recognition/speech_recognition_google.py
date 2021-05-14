import speech_recognition as sr

recognizer_instance = sr.Recognizer()

with sr.Microphone() as source:
    recognizer_instance.adjust_for_ambient_noise(source)
    print("Sono in ascolto... parla pure!")
    audio = recognizer_instance.listen(source)
    print("ok! sto elaborando il messaggio!")

try:
    text = recognizer_instance.recognize_google(audio, language="it-IT")
    print("Google ha capito: \n", text)
except Exception as e:
    print(e)
