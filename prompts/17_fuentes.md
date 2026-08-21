# PROMPT 17 — AUMENTO DE FUENTE EN PANELES INFORMATIVOS

## PROYECTO

AstroBit — Unity

---

# CONTEXTO

El Prompt 16 realizó el ajuste final del panel de actividades/preguntas.

El resultado actual es correcto y **NO debe modificarse el Panel 2**, que corresponde a la actividad/pregunta.

Actualmente, al examinar cada componente del mapa, aparecen diferentes paneles de información:

1. **Panel 1:** información inicial del componente al examinarlo.
2. **Panel 2:** actividad/pregunta con las opciones de respuesta.
3. **Panel 3:** información/resultado posterior relacionado con el componente.

El problema actual es exclusivamente visual:

* La fuente del **Panel 1** es demasiado pequeña.
* La fuente del **Panel 3** también es demasiado pequeña.
* Ambos paneles tienen espacio disponible suficiente y la información podría mostrarse con una fuente ligeramente mayor.
* Como referencia visual debe utilizarse el tamaño actual de la fuente de la **pregunta del Panel 2**.

Quiero mejorar únicamente la legibilidad de los textos informativos.

---

# OBJETIVO ÚNICO

Aumentar **ligeramente** el tamaño de la fuente de los textos del:

* **Panel 1 — información inicial**
* **Panel 3 — información posterior/resultado**

La referencia para decidir el tamaño debe ser la fuente actual de la **pregunta del Panel 2**.

El aumento debe ser moderado.

**NO quiero que se haga un aumento agresivo que pueda provocar recortes o desbordamientos.**

La intención es que los textos del Panel 1 y Panel 3 se vean claramente más grandes y cómodos de leer, aprovechando el espacio que actualmente queda vacío.

---

# RESTRICCIÓN ABSOLUTA — PANEL 2

## NO TOCAR EL PANEL 2

El Panel 2 corresponde a la actividad/pregunta y fue corregido en el Prompt 16.

Debe permanecer **exactamente igual**.

NO modificar:

* Tamaño de fuente de la pregunta.
* Posición de la pregunta.
* Tamaño del RectTransform.
* Anchura.
* Altura.
* `horizontalOverflow`.
* `verticalOverflow`.
* Botones.
* Opciones de respuesta.
* Posición de botones.
* Título.
* Subtítulo.
* Botón Cerrar.
* Botón Continuar.
* Layout general.
* Tamaño general del panel.
* Lógica de actividades.
* Contador `X/8`.
* Feedback de respuestas.
* Actividad Final.

La configuración del Panel 2 debe considerarse **fuera del alcance de este prompt**.

---

# CAMBIO PERMITIDO

## PANEL 1

Localiza el texto que muestra la información inicial del componente al examinarlo.

Aumenta **ligeramente** su tamaño de fuente.

Usa como referencia el tamaño actual de la pregunta del Panel 2.

La idea visual debe ser:

> Panel 1: fuente ligeramente mayor que la actual y más cómoda de leer.

No modificar innecesariamente:

* contenido del texto;
* posición;
* tamaño del panel;
* botones;
* lógica;
* interacción;
* animaciones.

El cambio principal debe ser **únicamente el tamaño de fuente**.

---

# PANEL 3

Localiza el texto correspondiente a la información posterior/resultado del componente.

Aumenta **ligeramente** su tamaño de fuente.

Usa nuevamente como referencia la fuente actual de la pregunta del Panel 2.

No modificar innecesariamente:

* contenido;
* posición;
* tamaño del panel;
* botones;
* lógica;
* interacción;
* flujo de actividades.

El cambio principal debe ser **únicamente el tamaño de fuente**.

---

# CRITERIO VISUAL

La comparación debe ser aproximadamente:

```text
ANTES

Panel 1:
[texto pequeño.........................]
[.....................................]
[.....................................]


Panel 2:
[Pregunta con fuente más grande]
[Pregunta con fuente más grande]
[Pregunta con fuente más grande]


Panel 3:
[texto pequeño.........................]
[.....................................]
```

Después:

```text
Panel 1:
[Texto ligeramente más grande........]
[....................................]
[....................................]


Panel 2:
[Pregunta con fuente actual]
[Pregunta con fuente actual]
[Pregunta con fuente actual]


Panel 3:
[Texto ligeramente más grande........]
[....................................]
[....................................]
```

El Panel 2 funciona como **referencia**, no como objetivo de modificación.

No es necesario que Panel 1 y Panel 3 tengan exactamente el mismo tamaño de fuente que la pregunta.

El objetivo es que exista una diferencia mucho menor que la actual y que los textos informativos sean cómodos de leer.

---

# REGLA IMPORTANTE

Antes de modificar cualquier cosa:

1. Identifica exactamente qué `Text`, `TextMeshProUGUI` o componente equivalente controla el texto del Panel 1.
2. Identifica exactamente qué `Text`, `TextMeshProUGUI` o componente equivalente controla el texto del Panel 3.
3. Identifica el tamaño actual de fuente utilizado por la pregunta del Panel 2 únicamente para usarlo como referencia.
4. Modifica solamente los tamaños de fuente correspondientes al Panel 1 y Panel 3.

Si existe un helper compartido que crea varios textos del panel, **NO cambies el helper global** si eso puede afectar al Panel 2.

En ese caso, realiza el ajuste de manera específica sobre los textos del Panel 1 y Panel 3.

---

# NO HACER

NO:

* modificar el Panel 2;
* cambiar el tamaño de la pregunta;
* mover la pregunta;
* mover botones;
* cambiar botones;
* cambiar contenido;
* reescribir preguntas;
* cambiar respuestas;
* cambiar el contador `X/8`;
* cambiar ObjectiveSystem;
* cambiar la lógica de actividades;
* cambiar zonas;
* cambiar interacción `[E]`;
* cambiar cámaras;
* cambiar HUD;
* cambiar tamaños generales de paneles;
* cambiar colores;
* cambiar fuentes tipográficas;
* cambiar estilos;
* cambiar márgenes;
* cambiar alineaciones salvo que sea estrictamente necesario para evitar un problema causado directamente por el aumento de fuente;
* hacer cambios no relacionados.

**El alcance es exclusivamente aumentar ligeramente la fuente del Panel 1 y Panel 3.**

---

# VERIFICACIÓN

Después del cambio, verifica visualmente en Unity:

1. Examinar un componente.
2. Confirmar que el Panel 1 muestra la información con una fuente ligeramente mayor.
3. Confirmar que el texto sigue entrando correctamente.
4. Pasar al Panel 2.
5. Confirmar que el Panel 2 está exactamente igual que antes.
6. Responder la actividad.
7. Confirmar que el Panel 3 muestra la información con una fuente ligeramente mayor.
8. Confirmar que el texto del Panel 3 tampoco se corta.
9. Confirmar que ningún botón quedó desplazado o superpuesto.
10. Confirmar que no se modificó ningún otro elemento visual.

Realiza una comprobación con varias actividades/componentes, no solamente con uno.

---

# CRITERIO DE ÉXITO

El trabajo se considera correcto si:

* El Panel 1 tiene una fuente ligeramente mayor y más legible.
* El Panel 3 tiene una fuente ligeramente mayor y más legible.
* El Panel 2 permanece **sin ningún cambio**.
* Ningún texto nuevo se corta.
* No aparecen solapamientos.
* Los botones permanecen donde estaban.
* No se modifica la lógica del juego.
* No aparecen errores ni warnings nuevos relacionados con el cambio.

---

# INFORME FINAL

Al terminar, entrega un informe breve indicando:

1. Archivo(s) modificados.
2. Qué elementos de texto fueron modificados.
3. Tamaño de fuente anterior y nuevo, si es posible determinarlo.
4. Confirmación explícita de que el Panel 2 **no fue modificado**.
5. Resultado de la verificación visual.
6. Si apareció algún problema o warning nuevo.

No realices cambios fuera del alcance indicado.
