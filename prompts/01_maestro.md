# PROMPT — TRANSFORMACIÓN PROFUNDA Y POLISH COMPLETO DE ASTROBIT

## CONTEXTO

La FASE 1 — Auditoría ya fue completada.

La FASE 2 — Fundamentos también fue implementada y probada:

* TextMeshPro.
* Configuración.
* Volumen.
* Sensibilidad.
* Invertir eje Y.
* Persistencia.
* SaveManager con JSON.
* Continuar/Nueva partida.
* Pausa.
* Integración con los sistemas existentes.

Ahora quiero cambiar completamente la forma de trabajar.

**NO quiero seguir trabajando por pequeñas subfases.**

Quiero que tomes el diagnóstico completo de la auditoría y el PROMPT MAESTRO como una hoja de ruta general y que comiences una **transformación profunda de AstroBit**.

Quiero que tú mismo analices, priorices, implementes, pruebes y corrijas.

Mi objetivo es que AstroBit pase de sentirse como un proyecto universitario básico a sentirse como:

# UN VIDEOJUEGO EDUCATIVO COMPLETO, POLIDO, INTERACTIVO Y PROFESIONAL.

No quiero simplemente agregar contenido.

Quiero mejorar el juego como un todo.

---

# 1. AUTONOMÍA DE IMPLEMENTACIÓN

A partir de ahora NO necesito que me pidas aprobación para cada cambio individual.

Quiero que tomes decisiones de diseño y programación por tu cuenta.

Si encuentras:

* un bug;
* una mala alineación;
* una interfaz fuera de lugar;
* una interacción pobre;
* una pantalla vacía;
* una transición brusca;
* código duplicado;
* una arquitectura mejorable;
* un sistema incompleto;
* un elemento visual poco profesional;
* una configuración que no funciona correctamente;
* una oportunidad clara de mejorar el gameplay;

**arréglalo.**

No necesitas esperar a que yo te diga exactamente qué línea cambiar.

Si una mejora es segura y está claramente dentro de la visión de AstroBit:

**IMPLEMENTA-LA.**

---

# 2. NO QUIERO UNA IMPLEMENTACIÓN MECÁNICA DEL PROMPT

El PROMPT MAESTRO no debe interpretarse como una lista de tareas que simplemente hay que marcar como completadas.

Quiero que uses criterio de:

* diseñador de videojuegos;
* programador Unity;
* diseñador UX/UI;
* diseñador de sistemas;
* diseñador de experiencias educativas.

Si una solución propuesta anteriormente no es la mejor solución después de inspeccionar el proyecto real:

**mejórala.**

Si descubres una mejora que no estaba mencionada en el prompt original pero claramente mejora AstroBit:

**propónla e impleméntala si es segura.**

---

# 3. REVISIÓN GLOBAL CONTINUA

Antes de cada gran conjunto de cambios, revisa el estado actual del proyecto para evitar trabajar sobre suposiciones.

Inspecciona cuando sea necesario:

```text
Assets/
Scripts/
Scenes/
Prefabs/
UI/
Audio/
Materials/
Fonts/
```

y los sistemas actualmente implementados.

No quiero que hagas cambios ciegos.

Quiero que entiendas cómo está actualmente AstroBit y que construyas sobre eso.

---

# 4. REGLA PRINCIPAL — MEJORAR TODO

Quiero una revisión global de:

### GAMEPLAY

### INTERACTIVIDAD

### UI/UX

### VISUAL

### AUDIO

### PROGRESIÓN

### EDUCACIÓN

### AMBIENTACIÓN

### CÁMARA

### CONTROLES

### MENÚS

### PAUSA

### CONFIGURACIÓN

### GUARDADO

### ESCENAS

### CÓDIGO

### RENDIMIENTO

No quiero que te concentres únicamente en una categoría.

---

# 5. EJEMPLO INMEDIATO — CONFIGURACIÓN

Actualmente el sistema de Configuración funciona, pero visualmente hay problemas.

Por ejemplo:

**el panel/elementos de Configuración no están correctamente contenidos dentro de su cuadro/panel.**

Esto NO debe considerarse simplemente "algo que yo tengo que reportar".

Quiero que revises TODA la interfaz visualmente y corrijas problemas de:

* posiciones;
* anchajes;
* tamaños;
* márgenes;
* escalado;
* alineación;
* clipping;
* overflow;
* botones;
* sliders;
* textos;
* jerarquía;
* espaciado;
* navegación.

Si un elemento está fuera del panel:

**corrígelo.**

Si un slider tiene un tamaño absurdo:

**corrígelo.**

Si un texto se sale de su contenedor:

**corrígelo.**

Si una pantalla se ve vacía:

**mejórala.**

No quiero limitarme a corregir únicamente los problemas que yo haya detectado.

---

# 6. ESTÁNDAR VISUAL

Quiero que todo AstroBit tenga una identidad visual coherente.

Debe sentirse:

* tecnológico;
* futurista;
* limpio;
* moderno;
* educativo;
* inmersivo;
* profesional.

No quiero:

* UI genérica de Unity;
* botones gigantes;
* texto desproporcionado;
* elementos pegados a los bordes;
* ventanas mal alineadas;
* fuentes inconsistentes;
* colores sin propósito;
* espacios vacíos sin intención.

Quiero una jerarquía visual clara.

Cada pantalla debe parecer diseñada deliberadamente.

---

# 7. MENÚ PRINCIPAL

Revisa y mejora el Main Menu completo.

Debe sentirse como el menú de un videojuego terminado.

Debe tener:

```text
ASTROBIT

Nueva partida
Continuar
Configuración
Créditos
Salir
```

Con:

* buena composición;
* fondo apropiado;
* jerarquía;
* animaciones sutiles;
* Hover;
* Pressed;
* Selected;
* sonidos;
* transiciones;
* tipografía consistente;
* navegación correcta.

No lo hagas gigantesco.

Quiero algo elegante.

---

# 8. CONFIGURACIÓN

Revisa completamente el sistema existente.

Debe verse correctamente dentro de su contenedor.

Incluye y verifica:

```text
AUDIO

Volumen maestro
Volumen de música
Volumen de efectos
Volumen de interfaz

CONTROLES

Sensibilidad
Sensibilidad horizontal/vertical si es viable
Invertir eje Y

GRÁFICOS

Resolución
Pantalla completa
VSync
Calidad
```

Solo incluye opciones que realmente funcionen.

Todos los valores deben:

* aplicarse inmediatamente;
* guardarse;
* restaurarse al reiniciar el juego;
* tener una presentación coherente.

No agregues opciones falsas.

---

# 9. GUARDADO

Revisa el sistema de guardado JSON existente.

Debe ser robusto.

Comprueba:

```text
Nueva partida
→ jugar
→ progreso
→ guardar
→ cerrar
→ abrir
→ Continuar
→ restaurar
```

Corrige cualquier inconsistencia que encuentres.

Si existe un estado visual que no se restaura correctamente:

**arréglalo.**

No aceptes estados donde el progreso lógico diga una cosa y el mundo visual diga otra.

El jugador debe sentir que está continuando exactamente su partida.

---

# 10. PAUSA

Mejora visual y funcionalmente la pausa.

Debe sentirse integrada al juego.

```text
PAUSA

Continuar
Configuración
Reiniciar sección
Volver al menú
```

Mantén el comportamiento correcto del botón seleccionado.

No reintroduzcas el bug visual que ya fue corregido.

---

# 11. INTERACCIÓN

Este es uno de los cambios más importantes.

Actualmente AstroBit depende demasiado de:

```text
acercarse
+
presionar E
+
leer
```

Quiero evolucionar esto.

Busca oportunidades para convertir componentes existentes en interacciones reales.

Por ejemplo:

```text
CPU
→ activar

ALU
→ inspeccionar / ejecutar operación

Registros
→ manipular información

Caché
→ observar estados

RAM
→ instalar / retirar / inspeccionar

Almacenamiento
→ buscar archivos

Terminal
→ ejecutar acciones
```

No conviertas todo en minijuegos complejos.

Las interacciones deben ser pequeñas pero significativas.

---

# 12. HACER QUE LA COMPUTADORA PAREZCA VIVA

Quiero que el escenario parezca un sistema informático funcionando.

Busca oportunidades para:

* luces;
* pantallas;
* indicadores;
* ventiladores;
* pulsos;
* actividad;
* pequeños movimientos;
* sonidos;
* partículas;
* estados activos/inactivos.

No llenes el escenario de basura.

Cada elemento debe tener sentido.

---

# 13. FLUJO DE DATOS

Implementa una representación visual clara del flujo de información.

Conceptualmente:

```text
ALMACENAMIENTO
      ↓
     RAM
      ↓
    CACHÉ
      ↓
  REGISTROS
      ↓
     ALU
      ↓
     CPU
```

Debe poder observarse.

Utiliza, según lo que mejor encaje:

* luces;
* partículas;
* líneas;
* pulsos;
* animaciones;
* indicadores;
* cambios de estado.

Quiero que el jugador pueda literalmente observar el concepto educativo.

---

# 14. TERMINALES

Aprovecha el sistema de almacenamiento existente.

El pack:

```text
Assets/Cosmic_Retro_Computer_1_FREE/
```

fue encontrado durante la auditoría y actualmente está sin utilizar.

Evalúa utilizarlo como parte de la identidad de los terminales.

No lo agregues simplemente como decoración.

Haz que tenga una función.

Por ejemplo:

```text
Terminal
    ↓
Encender
    ↓
Interfaz
    ↓
Seleccionar archivo
    ↓
Ejecutar / cargar
    ↓
Enviar información
```

Si encuentras una idea mejor:

**hazla.**

---

# 15. SISTEMA DE ARCHIVOS

Aprovecha:

* FileShelf;
* StorageServer;
* LocationZone;
* StorageMission.

Quiero que el almacenamiento deje de ser simplemente una colección de objetos.

Debe representar conceptualmente:

```text
DISCO
 ↓
ARCHIVOS
 ↓
SELECCIÓN
 ↓
CARGA
 ↓
RAM
 ↓
CPU
```

La interacción debe enseñar algo.

---

# 16. OBJETIVOS Y PROGRESIÓN

Mantén `ObjectiveSystem`.

No lo reemplaces innecesariamente.

Pero mejora su presentación.

Quiero que el jugador sepa:

* qué está haciendo;
* por qué;
* qué acaba de conseguir;
* qué viene después.

Ejemplo:

```text
OBJETIVO

Comprender la ALU

✓ Registros
✓ Unidad de Control
→ ALU
○ Caché
```

Evita inundar la pantalla.

---

# 17. ENCICLOPEDIA

Si encaja con la arquitectura actual, implementa una:

```text
ENCICLOPEDIA
```

con conceptos descubiertos.

Por ejemplo:

```text
CPU ✓
ALU ✓
REGISTROS ✓
RAM ✓
CACHÉ ?
SSD ?
BUS ?
```

Debe ser opcional.

No debe interrumpir constantemente el gameplay.

---

# 18. INDICADORES DE INTERACCIÓN

Mejora el sistema:

```text
[E] Interactuar
```

para que sea visualmente claro.

Idealmente:

```text
[E]

INTERACTUAR

Terminal
```

o una variante más elegante.

Debe:

* aparecer al acercarse;
* desaparecer al alejarse;
* indicar la acción;
* animarse sutilmente;
* no saturar la pantalla.

---

# 19. INSPECCIÓN

Permite inspeccionar objetos importantes.

La información debe ser contextual.

No quiero grandes bloques de texto.

Quiero:

```text
OBJETO
Estado
Función
Dato importante
```

y que el jugador vuelva rápidamente al gameplay.

---

# 20. AUDIO

Mantén el `MusicManager` existente.

NO rompas la configuración actual que solucionó los tirones.

La música debe:

* persistir entre escenas;
* no duplicarse;
* hacer loop;
* respetar volumen;
* no generar stuttering.

Amplía el sistema con:

* SFX de interfaz;
* interacción;
* confirmación;
* error;
* objetivo completado;
* terminal;
* activación de sistemas.

Utiliza los canales de audio correctamente.

No reproduzcas sonidos innecesariamente.

---

# 21. CÁMARA Y CONTROLES

Revisa la cámara actual.

No reemplaces Cinemachine sin necesidad.

La sensibilidad debe sentirse natural.

El jugador debe poder:

* controlar sensibilidad;
* invertir Y;
* jugar cómodamente.

Si encuentras problemas de cámara:

**corrígelos.**

---

# 22. AMBIENTACIÓN

Revisa las habitaciones existentes.

No quiero habitaciones que parezcan simples cajas con objetos.

Busca oportunidades para:

* iluminación;
* pantallas;
* cables;
* terminales;
* indicadores;
* actividad;
* props;
* elementos funcionales;
* animaciones.

Todo debe tener relación con una computadora.

---

# 23. POLISH VISUAL

Revisa:

* iluminación;
* URP;
* Bloom;
* Tonemapping;
* Vignette;
* Motion Blur;
* sombras;
* materiales;
* anti-aliasing;
* partículas;
* TextMeshPro;
* Canvas Scaler;
* resolución;
* composición.

No aumentes efectos simplemente porque "se ve más bonito".

Prioriza calidad visual con rendimiento razonable.

---

# 24. TRANSICIONES

Añade transiciones cuando aporten calidad:

```text
Main Menu
→ Juego

Juego
→ Pausa

Pausa
→ Configuración

Configuración
→ Juego

Juego
→ Main Menu
```

Utiliza fades y animaciones sutiles.

No hagas transiciones lentas o molestas.

---

# 25. CÓDIGO Y ARQUITECTURA

Mientras trabajas, corrige problemas evidentes de arquitectura.

Prioridad:

1. bugs;
2. inconsistencias;
3. código peligroso;
4. duplicación significativa;
5. mantenibilidad.

Puedes introducir:

* UIFactory;
* PersistentSingleton;
* helpers;
* componentes reutilizables;

si realmente mejoran el proyecto.

Pero NO refactorices por estética.

No quiero romper código funcional por intentar "limpiarlo".

---

# 26. RENDIMIENTO

MUY IMPORTANTE.

AstroBit ya sufrió problemas de tirones anteriormente.

No vuelvas a introducirlos.

Después de implementar sistemas importantes revisa:

* FPS;
* CPU;
* memoria;
* GC;
* audio;
* carga de escena;
* UI.

Evita:

```csharp
FindObjectOfType
```

repetidamente.

Evita trabajo innecesario en `Update()`.

Evita crear/destruir objetos constantemente.

Si implementas partículas o elementos repetitivos, considera pooling cuando realmente sea necesario.

No sacrifiques rendimiento por efectos visuales.

---

# 27. ESPAÑOL

Todo contenido NUEVO debe estar en español.

No es necesario traducir sistemas antiguos si hacerlo implica riesgo o pérdida de tiempo.

Pero cualquier elemento nuevo debe utilizar:

```text
Español
```

correctamente.

No quiero mezclas como:

```text
Settings
Continue
Volume
Interact
```

en sistemas nuevos.

---

# 28. ACCESIBILIDAD

Mantén:

* volumen independiente;
* sensibilidad;
* inversión Y;
* buena legibilidad;
* contraste;
* feedback visual;
* subtítulos cuando corresponda.

No sacrifiques estética por accesibilidad.

Busca equilibrio.

---

# 29. REGLA DE ORO

Cada cambio debe responder al menos a una de estas preguntas:

> ¿Hace AstroBit más divertido?

> ¿Hace AstroBit más inmersivo?

> ¿Hace AstroBit más educativo?

> ¿Hace AstroBit más profesional?

Si no cumple ninguna:

**NO LO IMPLEMENTES.**

---

# 30. NO QUIERO "CHECKLIST DEVELOPMENT"

No quiero que simplemente hagas:

```text
☑ Settings
☑ Save
☑ Terminal
☑ UI
☑ Audio
```

y declares terminado.

Quiero que evalúes el resultado como EXPERIENCIA.

Después de implementar cambios, entra en Play Mode y piensa:

> "Si yo fuera un jugador viendo esto por primera vez, ¿qué se siente barato, vacío, confuso o poco profesional?"

Y corrígelo.

Haz varias iteraciones si es necesario.

---

# 31. ALCANCE

No quiero dividir el trabajo en pequeñas fases.

Quiero realizar **grandes cambios en pocos ciclos de trabajo**.

Puedes trabajar en varios sistemas relacionados en una misma sesión.

No necesitas preguntarme:

> "¿Puedo arreglar este botón?"

> "¿Puedo mejorar este panel?"

> "¿Puedo añadir este feedback?"

Si la respuesta es claramente sí dentro de esta visión:

**hazlo.**

---

# 32. PERO NO DESTRUYAS

La autonomía NO significa modificar todo indiscriminadamente.

Conserva:

* PlayerInteraction;
* IInteractable;
* GameHUD;
* ObjectiveSystem;
* EducationalInteractable;
* StorageMission;
* MissionStepPoint;
* MissionNavigation;
* LocationZone;
* MusicManager;
* movimiento;
* cámara;
* progresión;
* escenas;
* sistema de archivos existente;
* sistema de pausa;
* guardado funcional.

Si necesitas modificar alguno:

1. entiende primero cómo funciona;
2. conserva su comportamiento;
3. cambia únicamente lo necesario;
4. verifica que siga funcionando.

---

# 33. NO ASUMIR

No inventes:

* assets;
* dependencias;
* referencias;
* APIs;
* sistemas existentes.

Si algo no existe:

compruébalo.

Si necesitas un asset externo:

**NO lo descargues ni agregues silenciosamente.**

Primero evalúa si puede conseguirse un resultado suficientemente bueno con los recursos existentes.

Prioriza los assets que ya están dentro del proyecto.

---

# 34. VALIDACIÓN CONTINUA

No quiero que llegues al final con 100 cambios y recién entonces descubras que algo se rompió.

Trabaja en grandes conjuntos coherentes.

Después de cada conjunto importante:

```text
Compilar
↓
Ejecutar
↓
Probar
↓
Detectar problemas
↓
Corregir
↓
Continuar
```

Si encuentras un bug provocado por tus propios cambios:

**corrígelo tú mismo.**

No me lo entregues como "limitación conocida" si razonablemente puede solucionarse.

---

# 35. INFORME

No necesito informes enormes después de cada modificación.

Quiero que trabajes y avances.

Cuando hayas terminado un gran ciclo de mejora, entrega:

## CAMBIOS REALIZADOS

Qué se mejoró.

## SISTEMAS NUEVOS

Qué se agregó.

## BUGS CORREGIDOS

Qué problemas encontraste y solucionaste.

## ARCHIVOS MODIFICADOS

Lista de archivos relevantes.

## VALIDACIÓN

Qué probaste.

## PROBLEMAS RESTANTES

Solo problemas reales que requieran intervención posterior.

## SIGUIENTE GRAN BLOQUE

Qué consideras que debería mejorarse después.

---

# 36. OBJETIVO FINAL

Quiero que AstroBit termine sintiéndose así:

```text
ABRO ASTROBIT
      ↓
MENÚ PROFESIONAL
      ↓
NUEVA PARTIDA
      ↓
ENTRO A UNA COMPUTADORA
      ↓
EXPLORO
      ↓
ENCUENTRO SISTEMAS
      ↓
INTERACTÚO
      ↓
ACTIVO COMPONENTES
      ↓
OBSERVO INFORMACIÓN
      ↓
RESUELVO PROBLEMAS
      ↓
DESBLOQUEO SISTEMAS
      ↓
VEO EL FLUJO DE DATOS
      ↓
APRENDO
      ↓
COMPRENDO
      ↓
PROGRESO
      ↓
GUARDO
      ↓
PUEDO VOLVER Y CONTINUAR
```

Quiero que el jugador piense:

> **"Estoy dentro de una computadora."**

Y después:

> **"Estoy aprendiendo cómo funciona."**

Y finalmente:

> **"Esto realmente parece un videojuego."**

---

# 37. MENTALIDAD

No busques simplemente cumplir requisitos.

Busca **calidad**.

No agregues 50 sistemas mediocres.

Haz 10 sistemas bien integrados.

No llenes las habitaciones de objetos.

Haz que parezcan funcionales.

No llenes la pantalla de texto.

Haz que la información aparezca cuando importa.

No agregues efectos porque sí.

Haz que cada efecto comunique algo.

No cambies código porque sí.

Cambia lo que realmente necesite mejorar.

---

# 38. COMIENZA AHORA

La auditoría ya está hecha.

La FASE 2 ya está implementada.

A partir de este momento quiero que continúes con la transformación completa de AstroBit siguiendo este documento y el PROMPT MAESTRO original.

**No quiero que te detengas para dividir el trabajo en pequeñas fases.**

Trabaja en grandes bloques.

Analiza → implementa → prueba → corrige → mejora → continúa.

Prioriza siempre:

**ESTABILIDAD + CALIDAD + GAMEPLAY + EDUCACIÓN + POLISH.**

Cuando termines un ciclo importante, entrégame el informe y continúa con el siguiente gran bloque cuando sea seguro hacerlo.

El objetivo no es que AstroBit tenga más cosas.

El objetivo es que **AstroBit sea un mejor juego.**
