# PROMPT 32 — Ajuste horizontal de botones del Main Menu

## OBJETIVO

Hacer únicamente un pequeño ajuste visual en el menú principal.

Actualmente los botones del menú:

- Nueva Partida
- Continuar
- Opciones
- Créditos
- Salir

están demasiado desplazados hacia la derecha.

Quiero moverlos **ligeramente hacia la izquierda** para que queden mejor alineados con el texto/título **"Aventuras dentro de tu computadora"** del menú.

La intención visual es que el bloque de botones quede más centrado respecto al contenido principal del menú, no pegado hacia la derecha del texto.

---

## CAMBIO SOLICITADO

Modificar únicamente la posición horizontal (`anchoredPosition.x`) de los 5 botones del Main Menu.

- Moverlos ligeramente hacia la izquierda.
- Mantener exactamente la misma posición vertical (`anchoredPosition.y`).
- Mantener exactamente el mismo tamaño/escala actual.
- Mantener el mismo orden.
- Mantener colores, fuentes, Outline, estados y textos.
- No modificar el fondo.
- No modificar el título "Aventuras dentro de tu computadora".
- No modificar el Canvas.
- No modificar el CanvasScaler.
- No modificar `MainMenuController.cs`.
- No modificar `SampleScene.unity`.
- No modificar ningún sistema de gameplay.

### IMPORTANTE

Antes de modificar, inspecciona la posición actual de los botones y determina un desplazamiento horizontal pequeño y razonable.

No quiero un cambio exagerado.

Como referencia visual:

> Los botones deben quedar aproximadamente debajo/centrados respecto al contenido de "Aventuras dentro de tu computadora", en lugar de quedar claramente desplazados hacia la derecha.

Mover el bloque aproximadamente un **pequeño porcentaje de su posición X actual**, ajustándolo visualmente si es necesario.

---

## REGLAS

1. No crear contenedores nuevos.
2. No cambiar la jerarquía del Canvas.
3. No cambiar nombres de GameObjects.
4. No tocar `MainMenuController.cs`.
5. No cambiar el tamaño de los botones.
6. No cambiar su posición vertical.
7. No cambiar ningún otro elemento del menú.
8. No tocar `SampleScene.unity`.
9. No modificar gameplay ni sistemas existentes.

---

## VERIFICACIÓN

Después del cambio:

1. Abrir `MainMenu.unity`.
2. Confirmar visualmente que los 5 botones se desplazaron ligeramente hacia la izquierda.
3. Confirmar que los botones siguen completamente visibles.
4. Confirmar que mantienen exactamente su tamaño y separación vertical.
5. Ejecutar Play Mode.
6. Confirmar que el menú sigue funcionando:
   - Nueva Partida → `SampleScene`
   - Continuar mantiene su comportamiento actual.
   - Opciones/Créditos mantienen su estado actual.
   - Salir mantiene su comportamiento actual.
7. Confirmar que no aparecieron errores ni warnings nuevos.

Al finalizar, dame un informe breve indicando:
- posición X anterior;
- posición X nueva;
- archivo modificado;
- resultado de la prueba.