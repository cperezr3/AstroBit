# PROMPT 05 — CORRECCIÓN DE ETIQUETA FLOTANTE BILLBOARD

Proyecto: AstroBit — Unity

CONTEXTO

El sistema EducationalInteractable de la ALU ya funciona correctamente.

La prueba manual confirmó que:

- El jugador puede acercarse al cubo ALU.
- La etiqueta flotante aparece.
- El sistema de proximidad funciona.
- PlayerInteraction funciona.
- El prompt [E] funciona.
- El panel educativo funciona.
- La actividad 12 + 7 funciona.
- La recompensa funciona.
- Cinemachine funciona correctamente.
- El jugador ahora inicia en el centro del mapa.
- La cámara funciona correctamente desde el nuevo spawn.

Por lo tanto, NO quiero modificar ninguna de esas partes.

ACLARACIÓN SOBRE LOS OBJETOS DEL SPAWN

En una sesión anterior se detectó que habían desaparecido:

- Wall BayDoor
- Wall BayDoor (1)
- Corridor Wide Corner Windows

Ya está aclarado: esos objetos correspondían al área del spawn anterior y fueron eliminados intencionalmente por mí al reorganizar el spawn.

NO los restaures.
NO investigues ese asunto.
NO intentes recuperarlos.
NO hagas cambios relacionados con ellos.

OBJETIVO DE ESTE PROMPT

Corregir únicamente la orientación visual de la etiqueta flotante que aparece encima de ALU.

PROBLEMA ACTUAL

La etiqueta existe y aparece, pero solo es legible desde determinadas direcciones.

Cuando el jugador llega desde otro lado del ALU, el texto queda de espaldas o deja de ser visible.

Quiero que la etiqueta funcione como un BILLBOARD 3D.

COMPORTAMIENTO DESEADO

La etiqueta debe:

1. Permanecer encima del ALU.
2. Mantener su posición relativa al ALU.
3. Mirar siempre hacia la Main Camera.
4. Ser legible desde cualquier dirección desde la que llegue el jugador.
5. Mantenerse vertical respecto al mundo.
6. No quedar acostada mirando hacia arriba o hacia abajo.
7. No quedar invertida.
8. No cambiar su escala.
9. No modificar la posición del ALU.
10. No modificar la posición del jugador.
11. No modificar Cinemachine.

Ejemplo conceptual:

                 CÁMARA
                    👁
                    │
                    │
              ┌─────────────┐
              │     ALU     │
              │ Unidad      │
              │ Aritmético- │
              │ Lógica      │
              └─────────────┘
                     ▲
                     │
                   CUBO ALU

Si el jugador rodea el cubo:

      👁 →  [ ALU ]
      
o

[ ALU ]  ← 👁

la etiqueta debe girar para seguir mirando a la cámara.

IMPLEMENTACIÓN

Primero inspecciona cómo EducationalInteractable crea y actualiza:

ALU_Label

No crees un sistema nuevo si ya existe uno funcional.

Si la etiqueta ya se actualiza cada frame, modifica únicamente su rotación.

La lógica debe ser equivalente a:

Vector3 direction =
    Camera.main.transform.position - label.transform.position;

Pero la orientación debe mantener Vector3.up como referencia vertical.

Puedes utilizar:

Quaternion.LookRotation(direction, Vector3.up)

o la orientación inversa si el Canvas/texto de Unity está mirando hacia el eje contrario.

IMPORTANTE:

Comprueba cuál orientación hace que el texto sea realmente visible.

No asumas que LookRotation directo es correcto para el Canvas.

PRUEBA OBLIGATORIA

Después de modificarlo:

1. Guardar la escena.
2. Entrar en Play Mode.
3. Acercarse al ALU desde el frente.
4. Confirmar que la etiqueta aparece.
5. Rodear completamente el ALU.
6. Probar aproximadamente:

   - frente
   - atrás
   - izquierda
   - derecha
   - diagonal izquierda
   - diagonal derecha

7. Confirmar que desde todas esas posiciones el texto:

   ALU
   Unidad Aritmético-Lógica

   permanece visible y correctamente orientado.

8. Probar también con la cámara ligeramente por encima del jugador.
9. Confirmar que el texto no queda acostado ni invertido.

RESTRICCIONES

NO modificar:

- PlayerInteraction
- MovementInput
- CharacterSkinController
- CinemachineBrain
- CM FreeLook1
- CameraTarget
- ObjectiveSystem
- IInteractable
- GameHUD
- Input System
- ProjectSettings
- posición del jugador
- posición del ALU

NO crear:

- nuevos sistemas de cámara
- nuevos sistemas de interacción
- nuevos Canvas globales
- nuevos managers

Solo corregir el billboard de EducationalInteractable.

VERIFICACIÓN FINAL

Al terminar, informa:

1. Cómo se creaba originalmente ALU_Label.
2. Qué causaba que el texto solo fuera visible desde un lado.
3. Qué modificación realizaste.
4. Si ahora mira correctamente a Main Camera.
5. Si funciona rodeando completamente el ALU.
6. Si la etiqueta conserva su posición y escala.
7. Si hubo errores o warnings nuevos en consola.

NO continúes todavía con Cache, Registros, RAM ni Storage.

Este paso debe dejar la ALU completamente funcional y visualmente correcta antes de replicar el sistema.