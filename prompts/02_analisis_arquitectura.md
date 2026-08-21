# Análisis de arquitectura — AstroBit

## Objetivo

Basándote en el diagnóstico realizado en `prompts/01_inspeccion_inicial.md`, realiza un análisis técnico de la arquitectura actual de AstroBit.

Esta fase es exclusivamente de análisis.

NO MODIFIQUES NADA EN UNITY.
NO MODIFIQUES SCRIPTS.
NO ELIMINES GAMEOBJECTS.
NO CREES GAMEOBJECTS.
NO CAMBIES CONFIGURACIÓN DEL PROYECTO.

## Contexto

AstroBit es un videojuego educativo desarrollado en Unity.

El jugador recorre un entorno que representa componentes de un sistema informático/procesador y aprende mediante interacción con los elementos del escenario.

Actualmente existe:

- Movimiento en tercera persona mediante CharacterController y Animator.
- Cámara Cinemachine FreeLook.
- Sistema de interacción mediante raycast + tecla E.
- SimpleInteractable.
- PlayerInteraction.
- ObjectiveSystem.
- GameHUD generado por código.
- Varias zonas/elementos educativos: ALU/AHU, Cache L1, Cache L2, Cache L3, Registros, Unidad de Control, RAM1 y RAM2.
- Assets externos utilizados principalmente para construir el entorno.
- Scripts de terceros provenientes de Jammo-Character.

## Analiza específicamente

### 1. Arquitectura de scripts

Inspecciona los scripts propios de AstroBit y determina:

- Responsabilidad de cada script.
- Dependencias entre scripts.
- Qué scripts están correctamente separados.
- Qué scripts concentran demasiadas responsabilidades.
- Qué scripts podrían reutilizarse.
- Qué scripts deberían refactorizarse.
- Qué scripts deberían permanecer intactos por pertenecer a terceros.

### 2. Player

Analiza la arquitectura actual del jugador:

- CharacterController.
- Animator.
- MovementInput.
- CharacterSkinController.
- Dependencias con Jammo-Character.
- Dependencias con la cámara.
- Dependencias con Input Manager.
- Posibles problemas futuros.

Determina si conviene mantener los scripts de Jammo temporalmente o reemplazarlos gradualmente.

NO los reemplaces todavía.

### 3. Cámara

Analiza:

- Main Camera.
- CinemachineBrain.
- CM FreeLook1.
- CM vcam1.
- Follow.
- LookAt.
- Binding Mode.
- Heading.
- Prioridades.

Determina cuál debería ser la arquitectura definitiva de cámara para un juego de tercera persona.

NO MODIFIQUES LA CÁMARA TODAVÍA.

### 4. Input

Analiza la mezcla actual entre:

- New Input System.
- Input Manager legacy.
- StandaloneInputModule.
- InputSystem_Actions.inputactions.

Determina qué arquitectura sería más conveniente para AstroBit y qué riesgos existen al migrar.

NO MIGRES NADA TODAVÍA.

### 5. Interacción

Analiza:

- PlayerInteraction.
- SimpleInteractable.
- Raycast.
- Feedback.
- Prompt.
- Eventos.
- One-shot interactions.

Determina si la arquitectura actual puede escalar correctamente a:

- CPU.
- Cache L1.
- Cache L2.
- Cache L3.
- Registros.
- Unidad de Control.
- RAM.
- Objetivos futuros.

### 6. ObjectiveSystem

Analiza la implementación actual y determina cómo debería evolucionar para soportar:

- Secuencia de objetivos.
- Objetivos completados.
- Objetivos pendientes.
- Progreso.
- Todos los objetivos completados.
- Final del recorrido.
- Posibles objetivos opcionales.
- Futuras misiones.

No implementes nada todavía.

### 7. HUD

Analiza GameHUD y determina si conviene:

- Mantenerlo temporalmente.
- Refactorizarlo.
- Migrarlo posteriormente a Canvas/UI diseñado.
- Separar lógica de presentación.

No modifiques el HUD.

### 8. Assets externos

Identifica qué scripts y sistemas pertenecen a:

- Jammo-Character.
- SciFi Warehouse Kit.
- ScifiOfficeLite.
- Otros assets externos.

Determina qué partes debemos evitar modificar directamente para reducir problemas futuros.

### 9. Arquitectura objetivo

Propón una arquitectura limpia y sencilla para AstroBit.

No quiero una arquitectura excesivamente compleja.

Prioriza:

- Simplicidad.
- Mantenibilidad.
- Modularidad.
- Bajo acoplamiento.
- Facilidad para ampliar el juego.
- Compatibilidad con Unity 6.
- Facilidad de depuración.
- Código entendible para un proyecto universitario.

No introduzcas patrones innecesarios solamente por seguir patrones de diseño.

## Resultado esperado

Entrega el análisis dividido exactamente en:

A. Arquitectura actual

B. Dependencias principales

C. Problemas arquitectónicos

D. Código que debemos conservar

E. Código que debemos modificar

F. Código de terceros que debemos evitar modificar

G. Decisiones recomendadas sobre cámara

H. Decisiones recomendadas sobre Input

I. Decisiones recomendadas sobre interacción

J. Decisiones recomendadas sobre objetivos

K. Decisiones recomendadas sobre HUD

L. Arquitectura objetivo propuesta

M. Orden recomendado de implementación

N. Riesgos y posibles regresiones

## Regla fundamental

No implementes ninguna recomendación.

Este documento solamente debe producir el análisis y el plan técnico que utilizaremos posteriormente para crear las tareas de implementación.