import streamlit as st
import firebase_admin
from firebase_admin import credentials, db
import os
import pandas as pd
import fitz 
ruta_credenciales = "firebase-adminsdk.json"
ruta_pdf_privacidad = "Pprivacidad.pdf"

if not firebase_admin._apps:
    if not os.path.exists(ruta_credenciales):
        st.error(f"The credential file was not found at the path: {ruta_credenciales}")
    else:
        credenciales = credentials.Certificate(ruta_credenciales)
        firebase_admin.initialize_app(credenciales, {
            'databaseURL': 'https://iberiansprint-19445-default-rtdb.europe-west1.firebasedatabase.app/'
        })

def obtener_top_25():
    try:
        referencia = db.reference('usuarios')
        usuarios = referencia.order_by_child('recordDistancia').limit_to_last(25).get()
        if not usuarios:
            return []
        lista_usuarios = [valor for clave, valor in usuarios.items()]
        lista_usuarios.sort(key=lambda x: x['recordDistancia'], reverse=True)
        return lista_usuarios
    except Exception as e:
        st.error(f"Error fetching data from Firebase Realtime Database: {e}")
        return []

def obtener_top_25_monedas():
    try:
        referencia = db.reference('usuarios')
        usuarios = referencia.order_by_child('monedas').limit_to_last(25).get()
        if not usuarios:
            return []
        lista_usuarios = [valor for clave, valor in usuarios.items()]
        lista_usuarios.sort(key=lambda x: x['monedas'], reverse=True)
        return lista_usuarios
    except Exception as e:
        st.error(f"Error fetching data from Firebase Realtime Database: {e}")
        return []

def obtener_logros_de_jugador(username):
    try:
        referencia = db.reference(f'usuarios/{username}/logros')
        logros = referencia.get()
        if not logros:
            return {}
        return logros
    except Exception as e:
        st.error(f"Error fetching data from Firebase Realtime Database: {e}")
        return {}

st.set_page_config(page_title="Scoreboard IberianSprint", page_icon=":trophy:", layout="wide")

with open("style.css") as f:
    st.markdown(f"<style>{f.read()}</style>", unsafe_allow_html=True)

st.title("🏆 Scoreboard IberianSprint 🏆", anchor=None)
st.header("Top 25 Players by Distance", anchor=None)

if st.button('Update'):
    st.experimental_rerun()

top_25 = obtener_top_25()

if top_25:
    datos = []
    for indice, usuario in enumerate(top_25, start=1):
        if indice == 1:
            emoji = "🥇"
        elif indice == 2:
            emoji = "🥈"
        elif indice == 3:
            emoji = "🥉"
        else:
            emoji = "🏅"
        datos.append([usuario['username'], usuario['recordDistancia'], usuario['monedas'], emoji])
    
    df = pd.DataFrame(datos, columns=["Player", "Distance (meters)", "Coins", "Medal"])
    st.table(df)
else:
    st.write("No data available.")

st.header("Top 25 Players by Coins", anchor=None)

top_25_monedas = obtener_top_25_monedas()

if top_25_monedas:
    datos_monedas = []
    for indice, usuario in enumerate(top_25_monedas, start=1):
        if indice == 1:
            emoji = "🥇"
        elif indice == 2:
            emoji = "🥈"
        elif indice == 3:
            emoji = "🥉"
        else:
            emoji = "🏅"
        datos_monedas.append([usuario['username'], usuario['monedas'], emoji])
    
    df_monedas = pd.DataFrame(datos_monedas, columns=["Player", "Coins", "Medal"])
    st.table(df_monedas)
else:
    st.write("No data available.")

st.sidebar.image("imagenes/LOGO.png", use_column_width=True)
st.sidebar.markdown("# Game Scoreboard")
st.sidebar.markdown("Check out the top players and their achievements.")
st.sidebar.markdown("[Política de Privacidad](#)", unsafe_allow_html=True)

st.sidebar.markdown("## Achievement Legend")
logros = [
    ("First Steps", 100),
    ("Consistent Runner", 500),
    ("Marathon Trainee", 1500),
    ("Endurance Runner", 2000),
    ("Expert Marathoner", 5000),
    ("Track Master", 10000),
    ("Marathon Hero", 20000),
    ("Track Champion", 25000)
]

for logro, metros in logros:
    st.sidebar.markdown(f"- **{logro}** - Run {metros} meters")

st.header("Search Player")
username = st.text_input("Player's Name")

if st.button('Search'):
    if username:
        logros_jugador = obtener_logros_de_jugador(username)
        if logros_jugador:
            st.write(f"{username}'s Achievements:")
            for logro, desbloqueado in logros_jugador.items():
                estado = "Unlocked" if desbloqueado else "Locked"
                st.write(f"- {logro}: {estado}")
        else:
            st.write(f"No achievements found for player {username}.")
    else:
        st.write("Please enter a player's name.")


st.sidebar.header("Política de Privacidad")
if st.sidebar.button('Ver Política de Privacidad'):
    if os.path.exists(ruta_pdf_privacidad):
        with open(ruta_pdf_privacidad, "rb") as f:
            pdf_document = fitz.open(f)
            for page_num in range(pdf_document.page_count):
                page = pdf_document[page_num]
                image = page.get_pixmap()
                img_data = image.tobytes()
                st.image(img_data)
    else:
        st.sidebar.error("El archivo PDF de la política de privacidad no se encontró.")

#COMANDO LANZAR APP - python -m streamlit run app.py
