﻿// Nu Game Engine.
// Copyright (C) Bryan Edds, 2013-2018.

namespace Nu
open System
open OpenTK
open SDL2
open Prime
open Nu

/// Describes a mouse button.
type [<Struct>] MouseButton =
    | MouseLeft
    | MouseCenter
    | MouseRight
    | MouseX1
    | MouseX2
    override this.ToString () = scstring this
    static member toEventName this = ((scstring this).Substring "Mouse".Length)

[<RequireQualifiedAccess>]
module MouseState =

    /// Convert a MouseButton to SDL's representation.
    let toSdlButton mouseButton =
        match mouseButton with
        | MouseLeft -> SDL.SDL_BUTTON_LEFT
        | MouseCenter -> SDL.SDL_BUTTON_MIDDLE
        | MouseRight -> SDL.SDL_BUTTON_RIGHT
        | MouseX1 -> SDL.SDL_BUTTON_X1
        | MouseX2 -> SDL.SDL_BUTTON_X2

    /// Convert SDL's representation of a mouse button to a MouseButton.
    let toNuButton mouseButton =
        match mouseButton with
        | SDL.SDL_BUTTON_LEFT -> MouseLeft
        | SDL.SDL_BUTTON_MIDDLE -> MouseCenter
        | SDL.SDL_BUTTON_RIGHT -> MouseRight
        | SDL.SDL_BUTTON_X1 -> MouseX1
        | SDL.SDL_BUTTON_X2 -> MouseX2
        | _ -> failwith "Invalid SDL mouse button."

    /// Check that the given mouse button is down.
    let isButtonDown mouseButton =
        let sdlMouseButton = toSdlButton mouseButton
        let sdlMouseButtonMask = SDL.SDL_BUTTON sdlMouseButton
        let (sdlMouseButtonState, _, _) = SDL.SDL_GetMouseState ()
        sdlMouseButtonState &&& sdlMouseButtonMask <> 0u

    /// Get the position of the mouse.
    let getPosition () =
        let (_, x, y) = SDL.SDL_GetMouseState ()
        Vector2i (x, y)

    /// Get the position of the mouse in floating-point coordinates.
    let getPositionF world =
        let mousePosition = getPosition world
        Vector2 (single mousePosition.X, single mousePosition.Y)

[<RequireQualifiedAccess>]
module KeyboardState =

    /// Check that the given keyboard key is down.
    let isKeyDown scanCode =
        let keyboardStatePtr = fst (SDL.SDL_GetKeyboardState ())
        let keyboardStatePtr = NativeInterop.NativePtr.ofNativeInt keyboardStatePtr
        let state = NativeInterop.NativePtr.get<byte> keyboardStatePtr scanCode
        state = byte 1

    /// Check that either ctrl key is down.
    let isCtrlDown () =
        isKeyDown (int SDL.SDL_Scancode.SDL_SCANCODE_LCTRL) ||
        isKeyDown (int SDL.SDL_Scancode.SDL_SCANCODE_RCTRL)

    /// Check that either alt key is down.
    let isAltDown () =
        isKeyDown (int SDL.SDL_Scancode.SDL_SCANCODE_LALT) ||
        isKeyDown (int SDL.SDL_Scancode.SDL_SCANCODE_RALT)

    /// Check that either shift key is down.
    let isShiftDown () =
        isKeyDown (int SDL.SDL_Scancode.SDL_SCANCODE_LSHIFT) ||
        isKeyDown (int SDL.SDL_Scancode.SDL_SCANCODE_RSHIFT)
        