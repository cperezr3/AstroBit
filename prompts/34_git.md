# RECUPERAR MÚSICA + RESTAURAR MAIN MENU COMO PRIMERA ESCENA

## CONTEXTO

Tengo actualmente dos estados de AstroBit:

### PROYECTO LOCAL ACTUAL

Es el proyecto que estoy utilizando actualmente.

Está funcionando de manera estable y **NO quiero perder sus cambios actuales**.

Actualmente:

* NO tiene la música de AstroBit.
* El Main Menu no está configurado correctamente como primera escena.
* El proyecto actualmente no presenta los tirones que tenía anteriormente.

Por lo tanto, este proyecto LOCAL debe ser nuestra **BASE PRINCIPAL**.

---

### REPOSITORIO GIT

El repositorio contiene una versión anterior de AstroBit que tiene:

* La música.
* El sistema `MusicManager` / `AudioManager`.
* El Main Menu correctamente configurado o utilizado como escena inicial.
* La música funcionando entre escenas.

Sin embargo, esa versión anteriormente presentó tirones periódicos.

Por eso:

# NO QUIERO RESTAURAR TODO EL REPOSITORIO.

Quiero recuperar únicamente:

1. **La música y todo lo estrictamente necesario para que funcione.**
2. **El Main Menu y su configuración como primera escena.**

Todo lo demás debe permanecer basado en el estado LOCAL ACTUAL.

---

# OBJETIVO FINAL

Cuando abra AstroBit y presione Play, quiero que SIEMPRE ocurra:

```text id="5qu8ij"
PLAY
  ↓
MAIN MENU
  ↓
Nueva Partida
  ↓
GAME
```

Nunca debe iniciar directamente en la escena del juego.

Además:

```text id="yn7q9f"
MAIN MENU
   ↓
Música AstroBit
   ↓
Nueva Partida
   ↓
GAME
   ↓
Misma música continúa
```

La música no debe reiniciarse al entrar al juego.

---

# FASE 1 — AUDITORÍA

## NO MODIFIQUES NADA TODAVÍA.

Primero compara:

```text id="b7k2dr"
PROYECTO LOCAL ACTUAL
VS
REPOSITORIO GIT
```

Investiga específicamente:

### MAIN MENU

Busca:

* escena del Main Menu;
* nombre exacto del archivo;
* ubicación;
* configuración de escena;
* `Build Settings` / `Build Profiles`;
* orden de escenas;
* escena marcada como índice `0`;
* scripts que cargan escenas;
* cualquier `SceneManager.LoadScene`.

Determina exactamente qué hacía que el repositorio iniciara correctamente en el Main Menu.

---

### MÚSICA

Busca:

* archivo de música;
* `MusicManager.cs`;
* `AudioManager`;
* `AudioSource`;
* configuración del AudioSource;
* configuración de importación del audio;
* referencias desde Main Menu;
* `DontDestroyOnLoad`;
* cualquier otro script relacionado.

---

# FASE 2 — INVESTIGAR LOS TIRONES

Antes de recuperar la música, compara la configuración del audio del repositorio.

Revisa:

```text id="f3ph5a"
Load Type
Compression Format
Quality
Preload Audio Data
Load In Background
AudioClip.length
tamaño del archivo
```

La pista debería ser aproximadamente de 5 minutos.

No quiero recuperar accidentalmente una versión de 2 horas.

---

## CAUSA DE LOS TIRONES

Anteriormente se sospechó de:

```text id="e6q9jd"
Streaming
+
Load In Background = OFF
```

pero NO asumas que esa es la causa definitiva.

Investiga y compara.

El proyecto LOCAL ACTUAL NO presenta esos tirones.

Por lo tanto, el comportamiento del proyecto LOCAL será nuestra referencia.

---

# FASE 3 — INTEGRAR LA MÚSICA

Después de identificar los archivos necesarios:

Integra únicamente:

```text id="0k6j7u"
Archivo de música
+
MusicManager
+
AudioSource/configuración
+
referencias estrictamente necesarias
```

No reemplaces todo `MainMenu.unity` si contiene cambios que no están relacionados con la música.

Si necesitas modificar la escena, hazlo de forma quirúrgica.

---

# FASE 4 — RESTAURAR MAIN MENU COMO ESCENA INICIAL

Esto es obligatorio.

El Main Menu debe ser la **primera escena que Unity carga al presionar Play**.

Verifica la configuración correspondiente a la versión actual de Unity:

* `Build Settings`
* o `Build Profiles`

La escena:

```text
MainMenu
```

debe estar en el índice:

```text
0
```

o ser la primera escena de la lista de escenas de ejecución.

No basta con que la escena exista en `Assets/`.

Debe estar configurada como la escena inicial real del proyecto.

---

# FASE 5 — FLUJO DE ESCENAS

Comprueba el flujo:

```text id="xq6c4z"
MAIN MENU
   │
   ├── Nueva Partida
   │       ↓
   │    GAME
   │
   ├── Continuar
   │       ↓
   │    GAME / progreso correspondiente
   │
   ├── Configuración
   │
   └── Salir
```

No rompas las opciones que ya existan.

---

# FASE 6 — MÚSICA PERSISTENTE

El sistema debe funcionar:

```text id="g7m8lq"
MAIN MENU
   🎵
    ↓
Nueva Partida
    ↓
GAME
   🎵
```

La música debe:

* continuar;
* no reiniciarse;
* no duplicarse;
* hacer loop;
* utilizar una sola instancia del MusicManager.

Si el usuario vuelve:

```text id="v5h3q2"
GAME
 ↓
ESC
 ↓
VOLVER AL MENÚ
```

la música debe seguir funcionando sin crear una segunda instancia.

---

# FASE 7 — NO REEMPLAZAR EL PROYECTO LOCAL

MUY IMPORTANTE.

NO ejecutar:

```text id="8atf3e"
git reset --hard
git checkout .
git pull
```

NO reemplazar:

```text id="c3n1z7"
Assets/
Scenes/
Scripts/
```

completos desde Git.

NO hacer un merge completo.

La integración debe ser selectiva.

---

# FASE 8 — PROTECCIÓN CONTRA LOS TIRONES

Antes de la integración:

```text id="f9h3sa"
LOCAL ACTUAL
→ comprobar que funciona sin tirones
```

Después:

```text id="2kq4p1"
LOCAL + MÚSICA
→ comprobar rendimiento
```

Si vuelven los tirones:

**DETÉN LA IMPLEMENTACIÓN.**

No sigas agregando cambios.

Investiga qué modificación introdujo el problema.

---

# FASE 9 — CONFIGURACIÓN RECOMENDADA DE MÚSICA

La configuración final debe ser apropiada para una pista de aproximadamente 5 minutos.

Como referencia:

```text id="9m8b6x"
Load Type: Streaming
Compression Format: Vorbis
Loop: ON
Volume: 0.25
Spatial Blend: 0
Load In Background: ON
```

Pero NO cambies estas opciones simplemente por seguir esta lista.

Comprueba primero la versión de Unity y el comportamiento real.

La prioridad es:

**estabilidad + rendimiento + calidad de audio.**

---

# FASE 10 — VALIDACIÓN OBLIGATORIA

Comprueba:

### Inicio

```text id="1y4qpl"
Abrir proyecto
↓
Play
↓
Main Menu
```

### Música

```text id="n6r3wk"
Main Menu
↓
música
↓
Nueva Partida
↓
Game
↓
música continúa
```

### Rendimiento

Jugar durante varios minutos.

Comprobar que NO aparecen:

* congelamientos;
* tirones periódicos;
* spikes evidentes;
* pausas al cargar el audio.

### Escenas

Probar:

```text id="8k9m2c"
Menu → Game
Game → Menu
Menu → Game
```

varias veces.

No debe aparecer música duplicada.

---

# INFORME FINAL

Cuando termines, dime:

## Main Menu

* nombre exacto de la escena;
* ubicación;
* cómo se configuró como primera escena;
* índice de escena.

## Música

* archivo recuperado;
* ubicación;
* duración;
* tamaño;
* configuración final;
* MusicManager utilizado.

## Integración

Lista EXACTAMENTE:

```text
Archivos recuperados
Archivos modificados
Archivos creados
```

## Rendimiento

Indica:

* si los tirones aparecieron;
* si desaparecieron;
* qué causa encontraste;
* qué solución aplicaste.

## Estado final

Confirma:

```text
[ ] Main Menu inicia primero
[ ] Música inicia en Main Menu
[ ] Música continúa al entrar al juego
[ ] Música hace loop
[ ] No hay duplicación
[ ] No hay tirones
[ ] Los cambios actuales del proyecto local se conservaron
```

---

# REGLA PRINCIPAL

El objetivo NO es recuperar el proyecto del repositorio.

El objetivo es:

```text id="3v7j5n"
PROYECTO LOCAL ACTUAL
        +
MAIN MENU CORRECTAMENTE CONFIGURADO
        +
MÚSICA
        +
MUSICMANAGER
        +
CONFIGURACIÓN ESTABLE
        ↓
ASTROBIT FUNCIONAL Y ESTABLE
```

**El proyecto LOCAL ACTUAL es la fuente de verdad.**

Git solamente será utilizado como fuente para recuperar la música y el Main Menu/configuración estrictamente necesaria.

Empieza por la auditoría.

**NO MODIFIQUES NADA hasta terminar la comparación.**
