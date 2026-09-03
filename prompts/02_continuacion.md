Unity ya está enfocado y el MCP está reconectado.

Continúa exactamente desde donde quedaste.

Primero:

1. Recarga `SampleScene.unity` desde disco para asegurarte de que Unity no conserve el estado in-memory anterior.
2. Verifica que hayan desaparecido los dos `StorageMission` fantasma.
3. Verifica que `TopRig` tenga nuevamente la rotación original.
4. Comprueba que `AmbientAudio` y `EmissiveToggle` sigan presentes y correctamente configurados.
5. Ejecuta una validación real en Unity antes de modificar o guardar nuevamente la escena.

Después de confirmar que la escena está limpia, continúa autónomamente con el bloque de TERMINALES.

Importante: el objetivo no es solamente añadir decoración. Quiero que AstroBit tenga **gameplay + decoración + una estética bonita y coherente**.

La terminal debe sentirse como una parte funcional de la computadora y, si la arquitectura actual lo permite, integrarse realmente con el flujo existente:

ALMACENAMIENTO → TERMINAL/COMPUTADORA → RAM → CACHÉ → REGISTROS → ALU → CPU

Aprovecha `Cosmic_Retro_Computer_1_FREE` si sus prefabs encajan visual y funcionalmente.

No conviertas todo en `[E] leer texto`. Quiero que la interacción tenga estados, feedback visual, animaciones, pantallas, luces, sonidos o acciones cuando tenga sentido.

Al mismo tiempo, no destruyas la lógica existente de `StorageMission`, `FileShelf`, `StorageServer`, `EducationalInteractable`, `ObjectiveSystem`, `SaveManager`, etc.

Trabaja en un bloque grande y coherente:

* inspecciona el pack;
* decide qué elementos sirven;
* intégralos;
* conecta la terminal con el gameplay existente;
* añade feedback visual;
* prueba;
* corrige;
* vuelve a probar.

No me preguntes si quiero que implementes cada parte. **Toma las decisiones de diseño tú mismo y continúa autónomamente.**

Y MUY IMPORTANTE:

* No guardes `SampleScene.unity` mientras Unity esté entrando/saliendo de Play Mode o mientras el estado runtime todavía esté siendo limpiado.
* Después de detener Play Mode, espera a que Unity termine completamente su limpieza antes de guardar.
* Antes de cualquier guardado importante, revisa `git diff` para detectar objetos runtime serializados accidentalmente.
* Si necesitas editar YAML manualmente, vuelve a cargar la escena desde disco y valida posteriormente en el Editor real.
* No sobrescribas el tuning original de Cinemachine con valores calculados en runtime.

Una vez terminado este bloque, continúa automáticamente con el siguiente bloque de mayor impacto para conseguir que AstroBit se sienta como un **videojuego educativo bonito, interactivo y pulido**, no como una colección de paneles.

No te detengas simplemente porque una parte visual no pueda verificarse; usa las herramientas disponibles y continúa cuando sea seguro.
