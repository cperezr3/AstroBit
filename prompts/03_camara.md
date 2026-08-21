# Corrección y verificación de cámara — AstroBit

## Objetivo

Corregir y verificar el sistema de cámara en tercera persona de AstroBit.

La cámara debe:

- seguir al jugador correctamente;
- mantener una perspectiva real de tercera persona;
- permitir rotación alrededor del jugador;
- mantener una inclinación natural;
- evitar comportamiento orbital incorrecto;
- evitar atravesar paredes cuando sea posible;
- mantener al jugador correctamente encuadrado;
- funcionar correctamente durante el movimiento.

## Contexto conocido

La escena utiliza:

- Main Camera.
- CinemachineBrain.
- CM FreeLook1.
- CM vcam1.
- CinemachineCollider.

Hallazgos anteriores:

- CM FreeLook1 tiene actualmente Binding Mode = WorldSpace (4).
- CM FreeLook1 tiene prioridad 10.
- CM vcam1 tiene prioridad 0.
- CM vcam1 sigue directamente a Jammo_Player.
- No se encontró código que cambie las prioridades de las cámaras.
- Main Camera contiene PlayerInteraction.
- El jugador utiliza Jammo-Character.
- El proyecto utiliza Unity 6000.4.8f1.

## FASE 1 — INSPECCIÓN

Antes de modificar cualquier cosa:

1. Usa el MCP de Unity.
2. Inspecciona Main Camera.
3. Inspecciona CinemachineBrain.
4. Inspecciona CM FreeLook1.
5. Inspecciona CM vcam1.
6. Inspecciona Jammo_Player.
7. Inspecciona CameraTarget.
8. Comprueba Follow y LookAt.
9. Comprueba Binding Mode.
10. Comprueba Heading.
11. Comprueba las prioridades.
12. Comprueba los rigs de CM FreeLook1.
13. Comprueba CinemachineCollider.
14. Comprueba si existen scripts que modifiquen las prioridades de las cámaras.

No hagas modificaciones durante esta fase.

## FASE 2 — PRUEBA

Entra en Play Mode utilizando MCP si es posible.

Comprueba:

- posición inicial de la cámara;
- orientación;
- distancia al jugador;
- seguimiento;
- rotación;
- comportamiento al mover al jugador;
- comportamiento al girar;
- comportamiento al mirar hacia atrás;
- colisiones con paredes;
- comportamiento al entrar en espacios cerrados.

Si la cámara funciona correctamente, no hagas cambios innecesarios.

Si se reproduce el problema asociado a Binding Mode = WorldSpace, procede con la corrección.

## FASE 3 — CORRECCIÓN

Si la inspección y prueba confirman que CM FreeLook1 presenta el problema esperado:

1. Cambia Binding Mode de WorldSpace (4) a SimpleFollowWithWorldUp (5).
2. Mantén el sistema Cinemachine FreeLook.
3. No reemplaces Cinemachine por una cámara personalizada.
4. No modifiques Jammo_Player.
5. No modifiques MovementInput.
6. No modifiques CharacterSkinController.
7. No cambies el sistema de Input.
8. No modifiques PlayerInteraction.

Mantén el Heading en TargetForward (2) salvo que la prueba demuestre que es necesario modificarlo.

No cambies múltiples parámetros de cámara simultáneamente sin necesidad.

## CM vcam1

Determina si CM vcam1 tiene actualmente alguna utilidad real.

Busca referencias en:

- Assets/Scripts/
- escenas;
- prefabs;
- otros scripts.

Si no tiene ninguna referencia ni función:

NO LA ELIMINES AUTOMÁTICAMENTE.

Primero informa que parece no utilizada y propone eliminarla como tarea separada.

## FASE 4 — VERIFICACIÓN POSTERIOR

Después de cualquier cambio:

1. Guarda los cambios de escena si corresponde.
2. Ejecuta Play Mode.
3. Comprueba nuevamente la cámara.
4. Comprueba movimiento del jugador.
5. Comprueba interacción con E.
6. Comprueba que PlayerInteraction continúa funcionando.
7. Comprueba la consola de Unity.
8. Confirma que no aparecieron errores nuevos.

## REGLAS

- No modificar scripts de terceros.
- No modificar Jammo-Character.
- No modificar ProjectSettings.
- No migrar Input.
- No crear un sistema de cámara nuevo.
- No eliminar CM vcam1 en esta tarea.
- No cambiar parámetros que no sean necesarios.
- No realizar refactors no relacionados con la cámara.

## RESULTADO

Al finalizar informa:

A. Estado inicial de la cámara.
B. Problema encontrado.
C. Cambios realizados.
D. Parámetros finales importantes.
E. Resultado de la prueba en Play Mode.
F. Estado de CM vcam1.
G. Estado de la consola.
H. Posibles problemas restantes.

La prioridad es conseguir una cámara de tercera persona estable sin romper el movimiento ni la interacción existentes.