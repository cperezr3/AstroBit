# PROMPT — SIGUIENTE GRAN BLOQUE: POLISH VISUAL + DECORACIÓN + GAME FEEL

Proyecto: **AstroBit — Unity 6 URP**

## CONTEXTO

Las fases anteriores ya dejaron funcionando y verificados:

* Movimiento.
* Cámara Cinemachine.
* Interacción.
* HUD.
* Objetivos y progresión.
* Sistema educativo.
* Storage → RAM → CPU.
* FileShelf.
* StorageServer.
* MissionStepPoint.
* MissionBeacon.
* FinalActivity.
* Pantalla de finalización.
* Menú principal.
* Pausa.
* Configuración.
* Persistencia.
* Guardado/carga.
* Música.
* Audio ambiental.
* Feedback visual de interacción.
* Feedback visual de misión.
* Migración de UI a TMP.
* Configuración gráfica.
* Credits/Exit.
* Post-processing URP.

También se investigó profundamente el comportamiento de `TopRig` de Cinemachine.

### IMPORTANTE SOBRE TOPRIG

La investigación confirmó que el cambio de Transform de `TopRig` después de salir de Play Mode es comportamiento interno/irrelevante de los rigs orbitales de Cinemachine.

Se verificó que:

* La cámara real queda correctamente orientada.
* `CinemachineVirtualCamera.State.RawOrientation` es correcto.
* La Main Camera queda correctamente orientada.
* El problema no afecta al gameplay.

Por lo tanto:

> **NO vuelvas a tocar, restaurar, bloquear ni modificar manualmente TopRig.**

Ignóralo completamente salvo que aparezca un problema real visible de cámara.

### IMPORTANTE SOBRE STORAGE MISSION GHOSTS

Los objetos persistentes creados accidentalmente al ejecutar pruebas mediante `.Instance` sí son un problema real.

Antes de guardar escenas:

* comprueba que no existan `StorageMission` duplicados/ghosts creados accidentalmente;
* no invoques singletons mediante `execute_code` de forma que puedan persistir fuera de Play Mode;
* no guardes escenas contaminadas con objetos de prueba.

---

# OBJETIVO DE ESTE BLOQUE

Ahora quiero dejar de hacer correcciones pequeñas y avanzar hacia la transformación visual real de AstroBit.

Quiero que trabajes de forma **autónoma y amplia**.

No me pidas confirmación para cada cambio.

Primero audita visualmente la escena y después implementa todo lo que tenga sentido dentro de este bloque.

La prioridad es:

> **hacer que AstroBit realmente parezca un videojuego educativo tecnológico, bonito, coherente e inmersivo.**

No quiero solamente más scripts.

Quiero mejorar:

* decoración;
* iluminación;
* composición;
* ambientación;
* identidad visual;
* sensación de escala;
* feedback;
* lectura de las zonas;
* sensación de que la computadora está viva;
* claridad educativa mediante el propio entorno.

---

# 1. AUDITORÍA VISUAL REAL

Inspecciona `SampleScene.unity` directamente.

Analiza:

* CPU.
* RAM.
* Storage.
* corredores.
* zonas de transición.
* iluminación.
* materiales.
* objetos decorativos existentes.
* espacios vacíos.
* escalas.
* composición desde la cámara real del jugador.
* zonas que parecen prototipos.
* zonas que parecen demasiado vacías.
* elementos que parecen colocados arbitrariamente.

No hagas una lista interminable.

Identifica primero las mejoras que realmente tendrían impacto visual.

---

# 2. IDENTIDAD VISUAL DE ASTROBIT

La escena debe sentirse como:

**"Estoy dentro de una computadora."**

No como:

**"Hay componentes de computadora gigantes colocados en una habitación."**

Usa el entorno para comunicar función.

Por ejemplo:

### CPU

Debe sentirse como el centro de procesamiento.

Puede utilizar:

* iluminación más intensa;
* LEDs;
* pequeños indicadores;
* líneas/cables;
* pulsos de actividad;
* elementos que sugieran procesamiento;
* decoración alrededor de ALU, registros y unidad de control;
* pantallas o indicadores donde tenga sentido.

### RAM

Debe sentirse como memoria activa.

Puede utilizar:

* iluminación dinámica;
* indicadores de módulos;
* pulsos;
* conexiones visuales;
* actividad luminosa;
* elementos repetitivos que comuniquen almacenamiento temporal.

### STORAGE

Debe sentirse como almacenamiento masivo.

Puede utilizar:

* racks;
* servidores;
* indicadores;
* luces de actividad;
* organización visual;
* elementos que comuniquen archivos/datos.

No agregues decoración aleatoria.

Cada elemento debe tener una razón estética o educativa.

---

# 3. DECORACIÓN

Busca espacios muertos y mejora la composición.

Considera elementos como:

* cables;
* conduits;
* paneles;
* luces;
* pequeños módulos electrónicos;
* ventilación;
* indicadores;
* racks;
* monitores;
* placas;
* soportes;
* estructuras;
* pequeños props tecnológicos;
* elementos de pared;
* iluminación ambiental.

Pero:

## NO SATURAR

No quiero llenar todo de objetos.

Debe existir:

* jerarquía visual;
* espacios negativos;
* puntos focales;
* caminos claros;
* lectura limpia.

La decoración debe parecer diseñada, no generada al azar.

---

# 4. ILUMINACIÓN

Haz una pasada seria de iluminación.

Evalúa:

* luces principales;
* luces ambientales;
* contraste;
* zonas oscuras;
* lectura de los componentes;
* iluminación de corredores;
* iluminación de objetivos;
* iluminación de zonas educativas.

Busca una estética:

**futurista + tecnológica + limpia + inmersiva.**

Evita:

* iluminación completamente plana;
* exceso de bloom;
* luces quemadas;
* colores aleatorios;
* zonas completamente negras.

La iluminación debe ayudar también a guiar al jugador.

---

# 5. "COMPUTADORA VIVA"

Quiero que el escenario tenga actividad.

Sin convertirlo en una feria de partículas.

Implementa, donde tenga sentido:

* LEDs parpadeando;
* pulsos suaves;
* pequeños cambios de emisión;
* indicadores de actividad;
* ventilación;
* actividad en servidores;
* pequeñas animaciones ambientales;
* señales de que los componentes están funcionando.

Estos efectos deben ser sutiles.

El jugador debería sentir:

> "Este lugar está funcionando."

---

# 6. DATA FLOW

Si el sistema visual de flujo de datos todavía no está implementado, empieza a implementarlo en este bloque.

La idea conceptual es:

**STORAGE → RAM → CACHE → REGISTROS → ALU → CPU**

No necesariamente tiene que ser una simulación física compleja.

Puede representarse mediante:

* pulsos luminosos;
* líneas;
* partículas;
* pequeños paquetes de energía;
* indicadores;
* animaciones.

Lo importante es que el jugador pueda visualizar:

> "Los datos se están moviendo por el sistema."

Integra este concepto con la progresión existente.

NO reemplaces la lógica educativa existente.

---

# 7. MEJORAR LOS OBJETIVOS VISUALMENTE

Los `MissionBeacon` actuales son un buen comienzo.

Hazlos evolucionar si realmente mejora el resultado.

Por ejemplo:

* pulso de luz;
* dirección visual;
* pequeña animación;
* feedback al completar;
* relación visual con la siguiente zona.

Pero evita convertir cada objetivo en un marcador gigante.

El objetivo debe ser descubrible, no gritárselo al jugador.

---

# 8. INTERACTIVIDAD + DECORACIÓN

Donde sea viable, combina ambos conceptos.

Ejemplo:

En lugar de:

> objeto decorativo + texto educativo

buscar:

> objeto decorativo → interactúa → se activa → cambia visualmente → explica su función.

Eso hace que el mundo sea parte del aprendizaje.

Prioriza esto en objetos importantes.

---

# 9. AUDIO AMBIENTAL

Ya existe audio ambiental con `Fan_St.wav`.

No lo reemplaces innecesariamente.

Evalúa si hace falta añadir pequeños efectos utilizando **únicamente recursos ya existentes en el proyecto**, por ejemplo:

* clicks;
* electricidad;
* actividad;
* máquinas;
* interfaces.

No inventes dependencias externas.

No descargues paquetes externos.

Si los recursos existentes no son suficientes, deja el sistema preparado y continúa con lo que sí pueda hacerse.

---

# 10. PERFORMANCE

No optimices a ciegas.

Primero revisa si las nuevas decoraciones o efectos pueden causar problemas.

Especial atención a:

* Update innecesarios;
* partículas excesivas;
* luces dinámicas excesivas;
* materiales duplicados;
* GameObjects innecesarios;
* objetos decorativos repetidos en cantidades absurdas.

La prioridad es:

**calidad visual sin destruir rendimiento.**

Si existe una mejora clara que reduce coste sin afectar visualmente, aplícala.

---

# 11. REGLAS IMPORTANTES

### NO HACER

* No reintroducir mini-quizzes antiguos.
* No cambiar el flujo Storage → RAM → CPU.
* No reemplazar sistemas funcionales.
* No modificar TopRig.
* No hacer refactors gigantes sin necesidad.
* No crear un GameManager monstruoso.
* No introducir dependencias externas.
* No descargar assets externos.
* No llenar la escena de decoración aleatoria.
* No convertir todo en partículas.
* No crear 50 scripts para cosas simples.
* No hacer microcambios sin impacto.

### SÍ HACER

* Cambios grandes y coherentes.
* Mejoras visuales reales.
* Decoración con propósito.
* Gameplay feedback.
* Ambiente.
* Identidad visual.
* Claridad educativa.
* Sensación de videojuego terminado.

---

# 12. VALIDACIÓN

Al terminar:

1. Compila el proyecto.
2. Abre `SampleScene`.
3. Entra en Play Mode.
4. Comprueba:

   * movimiento;
   * cámara;
   * interacción;
   * HUD;
   * progresión;
   * Storage;
   * RAM;
   * CPU;
   * misión;
   * feedback;
   * pantalla final.
5. Comprueba visualmente:

   * CPU;
   * RAM;
   * Storage;
   * corredor;
   * iluminación;
   * decoración;
   * composición.
6. Revisa Console.
7. Comprueba que no existan `StorageMission` ghosts.
8. Guarda la escena solamente cuando estés seguro de que está limpia.
9. Comprueba el diff final de Git.

Si algo se rompe, corrígelo dentro de este mismo bloque.

---

# 13. CRITERIO DE ÉXITO

No consideres terminado el bloque simplemente porque:

> "compila."

Debe terminar cuando el resultado visual sea claramente mejor.

Quiero poder entrar a `SampleScene` y notar inmediatamente:

* más identidad;
* más profundidad;
* mejor iluminación;
* mejor composición;
* más actividad;
* más inmersión;
* mejor lectura de las zonas;
* más sensación de videojuego.

Y, sobre todo:

> **que la decoración y los efectos hagan que el jugador entienda mejor que está dentro de una computadora.**

Trabaja de forma autónoma.

Haz primero la auditoría y después implementa el bloque completo.

No me pidas autorización para cada mejora individual.

Al final entrégame un informe con:

1. Qué analizaste.
2. Qué cambiaste.
3. Qué objetos/escenas/scripts modificaste.
4. Qué mejoras visuales implementaste.
5. Qué mejoras de gameplay/feedback implementaste.
6. Qué quedó pendiente.
7. Validaciones realizadas.
8. Estado de compilación.
9. Estado de la Console.
10. Estado de Git/diff.
11. Qué recomiendas como **siguiente gran bloque**.
