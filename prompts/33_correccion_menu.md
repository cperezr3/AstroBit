# PROMPT — SISTEMA DE MÚSICA GLOBAL Y CORRECCIÓN DE TIRONES

Proyecto: **AstroBit — Unity**

Quiero que implementes y dejes completamente funcional el sistema de música de AstroBit.

## CONTEXTO ACTUAL

Actualmente tengo una pista musical para AstroBit:

`AstroBit_MainTheme`

La música ya está importada en el proyecto y actualmente tengo un `Audio Source` creado manualmente dentro de la **escena del menú principal**.

La configuración actual del Audio Source es aproximadamente:

* Audio Generator: `AstroBit_MainTh...`
* Output: `None`
* Mute: desactivado
* Bypass Effects: desactivado
* Bypass Listener Effects: desactivado
* Play On Awake: activado
* Loop: activado
* Priority: `128`
* Volume: `1`
* Pitch: `1`
* Stereo Pan: `0`
* Spatial Blend: `0`
* Reverb Zone Mix: `1`

## PROBLEMAS ACTUALES

### Problema 1 — La música solamente funciona en el menú

El Audio Source está dentro de la escena del menú principal.

Cuando cambio a la escena del juego, ese objeto desaparece y por eso la música deja de sonar.

Quiero que la música pueda continuar al pasar:

`Menú principal → Juego`

sin reiniciarse ni desaparecer.

---

### Problema 2 — Tirones/congelamientos durante el juego

Al reproducir la música actualmente se producen tirones periódicos durante el gameplay.

El juego parece congelarse brevemente aproximadamente cada 2 segundos.

Quiero que investigues la causa antes de modificar código innecesariamente.

Determina si el problema está relacionado con:

* configuración de importación del audio;
* carga del AudioClip;
* `Audio Source`;
* streaming;
* compresión;
* memoria;
* `Preload Audio Data`;
* alguna configuración incorrecta del archivo;
* o algún otro sistema/script de AstroBit.

NO asumas automáticamente que el Audio Source es la causa.

Si es necesario, haz una prueba/desactivación temporal para determinar si los tirones desaparecen cuando la música está desactivada.

---

# OBJETIVO FINAL

Quiero terminar con un sistema sencillo, estable y apropiado para un proyecto universitario.

La arquitectura deseada es:

```text
ESCENA MENÚ
    │
    └── MusicManager
            │
            └── Audio Source
                    │
                    └── AstroBit_MainTheme
                            │
                            ├── Play On Awake
                            ├── Loop
                            ├── Volume ≈ 0.25
                            └── Spatial Blend = 0
                                  │
                                  ↓
                         CAMBIO DE ESCENA
                                  │
                                  ↓
                              ESCENA GAME
                                  │
                         MusicManager continúa
```

El sistema debe utilizar `DontDestroyOnLoad` correctamente para que la música sobreviva al cambio de escena.

---

# IMPLEMENTACIÓN

## 1. Crear un MusicManager

Crea un script apropiado, por ejemplo:

`MusicManager.cs`

Debe:

* existir una única instancia;
* utilizar correctamente el patrón Singleton;
* utilizar `DontDestroyOnLoad(gameObject)`;
* evitar duplicados cuando se carguen otras escenas;
* destruir automáticamente una instancia duplicada;
* mantener la reproducción de la música durante los cambios de escena.

No crees múltiples MusicManagers innecesarios.

---

## 2. Audio Source

Configura el Audio Source para música de fondo:

```text
Play On Awake: ON
Loop: ON
Volume: 0.25 aproximadamente
Pitch: 1
Stereo Pan: 0
Spatial Blend: 0
```

No quiero que la música sea 3D ni que dependa de la posición del jugador.

Debe ser música global.

---

# 3. CONFIGURACIÓN DEL ARCHIVO DE AUDIO

Revisa el archivo:

`AstroBit_MainTheme`

y verifica su duración real.

IMPORTANTE:

Anteriormente se utilizó accidentalmente una versión de aproximadamente **2 horas**, que provocaba una importación extremadamente larga.

Esa versión ya fue recortada.

CONFIRMA que Unity está utilizando realmente la versión recortada.

No vuelvas a utilizar la versión de 2 horas.

Para la música del juego, utiliza una configuración adecuada para reproducción continua.

Evalúa las opciones disponibles según la versión de Unity utilizada, especialmente:

* Load Type
* Compression Format
* Quality
* Preload Audio Data
* Load In Background

Para una pista musical relativamente larga, prioriza una configuración que permita reproducción eficiente y evite cargar innecesariamente todo el archivo en memoria.

Si `Streaming` es la opción adecuada para la duración actual de la pista, utilízala.

No cambies configuraciones arbitrariamente: explica brevemente por qué eliges cada una.

---

# 4. INVESTIGAR LOS TIRONES

Este punto es MUY IMPORTANTE.

Antes de declarar terminado el trabajo, investiga los tirones.

Comprueba:

### A. ¿Los tirones desaparecen sin música?

Desactiva temporalmente el Audio Source y ejecuta el juego.

Si los tirones desaparecen:

→ el problema probablemente está relacionado con el audio/importación/carga.

Si continúan:

→ investiga otros scripts o sistemas que puedan ejecutarse periódicamente.

### B. Revisa scripts existentes

No reemplaces sistemas existentes sin necesidad.

Revisa especialmente scripts que puedan tener:

* `Update()`;
* `InvokeRepeating`;
* coroutines;
* búsquedas frecuentes;
* `Find`;
* `FindObjectOfType`;
* instanciación/destrucción periódica;
* operaciones de archivos;
* carga de recursos;
* llamadas repetitivas que puedan producir Garbage Collection.

NO modifiques sistemas funcionales de AstroBit simplemente por precaución.

El objetivo es encontrar la causa real.

---

# 5. EVITAR DUPLICACIÓN DE MÚSICA

Cuando se cambie entre escenas:

```text
Menu
↓
Game
↓
otras escenas
```

NO deben existir múltiples Audio Sources reproduciendo la misma canción.

Debe existir solamente una instancia válida del MusicManager.

Si el menú vuelve a cargarse, tampoco debe aparecer una segunda canción encima de la primera.

---

# 6. NO ROMPER EL PROYECTO

MUY IMPORTANTE:

Antes de modificar scripts:

1. Revisa cómo están organizadas actualmente las escenas.
2. Revisa cómo se realiza actualmente el cambio de escena.
3. Revisa si ya existe algún sistema de audio.
4. Revisa si ya existe algún GameManager.
5. Revisa si existe algún objeto persistente mediante `DontDestroyOnLoad`.

Si ya existe una arquitectura equivalente, reutilízala en lugar de crear otra.

NO dupliques responsabilidades.

NO cambies:

* PlayerInteraction
* GameHUD
* ObjectiveSystem
* EducationalInteractable
* StorageMission
* sistemas de movimiento
* cámaras
* UI
* progresión educativa

salvo que encuentres una dependencia directa y necesaria con el problema de audio/rendimiento.

---

# 7. RESULTADO ESPERADO

Al terminar quiero poder hacer:

```text
1. Abrir AstroBit
2. Entrar al menú principal
3. La música comienza automáticamente
4. Presionar Jugar
5. Entrar al gameplay
6. La música continúa
7. La música hace loop cuando termina
8. No aparecen dos canciones simultáneamente
9. La música tiene un volumen aproximado de 25%
10. El gameplay no presenta los tirones periódicos anteriores
```

---

# 8. VALIDACIÓN OBLIGATORIA

No quiero que solamente escribas el código.

Después de implementar:

### Comprueba:

* que el proyecto compile sin errores;
* que `MusicManager.cs` no tenga errores;
* que el objeto persista entre escenas;
* que no se duplique;
* que la música continúe después de cambiar de escena;
* que el audio esté configurado correctamente;
* que la pista utilizada sea la versión recortada;
* que los tirones hayan sido investigados.

Si no puedes ejecutar Unity para comprobar algo, indícalo claramente y dime exactamente qué debo probar manualmente.

---

# 9. INFORME FINAL

Cuando termines, entrégame un informe breve con:

### Archivos modificados

Lista cada archivo creado/modificado.

### Cambios realizados

Explica qué cambiaste.

### Configuración final del audio

Indica:

```text
Load Type:
Compression Format:
Quality:
Preload Audio Data:
Load In Background:
Loop:
Volume:
Spatial Blend:
```

### Diagnóstico de los tirones

Indica:

* causa encontrada;
* cómo la comprobaste;
* solución aplicada.

### Prueba manual

Dime exactamente qué debo hacer en Unity para verificar que todo quedó funcionando.

NO hagas cambios adicionales fuera del alcance de este problema.
