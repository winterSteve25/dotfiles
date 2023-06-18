// Decompiled with JetBrains decompiler
// Type: Raylib_cs.Raylib
// Assembly: Raylib-cs, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: A6FB1724-C984-42FB-BC6B-1E563BC80FC5
// Assembly location: /home/cadenz/.nuget/packages/raylib-cs/4.5.0/lib/net6.0/Raylib-cs.dll
// XML documentation location: /home/cadenz/.nuget/packages/raylib-cs/4.5.0/lib/net6.0/Raylib-cs.xml

using System;
using System.Numerics;
using System.Runtime.InteropServices;
using System.Security;

namespace Raylib_cs
{
  [SuppressUnmanagedCodeSecurity]
  public static class Raylib
  {
    /// <summary>Used by DllImport to load the native library</summary>
    public const string nativeLibName = "raylib";
    public const string RAYLIB_VERSION = "4.5";
    public const float DEG2RAD = 0.017453292f;
    public const float RAD2DEG = 57.295776f;

    /// <summary>
    /// Get color with alpha applied, alpha goes from 0.0f to 1.0f<br />
    /// NOTE: Added for compatability with previous versions
    /// </summary>
    public static Color Fade(Color color, float alpha) => Raylib.ColorAlpha(color, alpha);

    /// <summary>Initialize window and OpenGL context</summary>
    [DllImport("raylib", CallingConvention = (CallingConvention) 2)]
    public static extern unsafe void InitWindow(int width, int height, sbyte* title);

    /// <summary>Check if KEY_ESCAPE pressed or Close icon pressed</summary>
    [DllImport("raylib", CallingConvention = (CallingConvention) 2)]
    public static extern CBool WindowShouldClose();

    /// <summary>Close window and unload OpenGL context</summary>
    [DllImport("raylib", CallingConvention = (CallingConvention) 2)]
    public static extern void CloseWindow();

    /// <summary>Check if window has been initialized successfully</summary>
    [DllImport("raylib", CallingConvention = (CallingConvention) 2)]
    public static extern CBool IsWindowReady();

    /// <summary>Check if window is currently fullscreen</summary>
    [DllImport("raylib", CallingConvention = (CallingConvention) 2)]
    public static extern CBool IsWindowFullscreen();

    /// <summary>Check if window is currently hidden (only PLATFORM_DESKTOP)</summary>
    [DllImport("raylib", CallingConvention = (CallingConvention) 2)]
    public static extern CBool IsWindowHidden();

    /// <summary>Check if window is currently minimized (only PLATFORM_DESKTOP)</summary>
    [DllImport("raylib", CallingConvention = (CallingConvention) 2)]
    public static extern CBool IsWindowMinimized();

    /// <summary>Check if window is currently maximized (only PLATFORM_DESKTOP)</summary>
    [DllImport("raylib", CallingConvention = (CallingConvention) 2)]
    public static extern CBool IsWindowMaximized();

    /// <summary>Check if window is currently focused (only PLATFORM_DESKTOP)</summary>
    [DllImport("raylib", CallingConvention = (CallingConvention) 2)]
    public static extern CBool IsWindowFocused();

    /// <summary>Check if window has been resized last frame</summary>
    [DllImport("raylib", CallingConvention = (CallingConvention) 2)]
    public static extern CBool IsWindowResized();

    /// <summary>Check if one specific window flag is enabled</summary>
    [DllImport("raylib", CallingConvention = (CallingConvention) 2)]
    public static extern CBool IsWindowState(ConfigFlags flag);

    /// <summary>Set window configuration state using flags</summary>
    [DllImport("raylib", CallingConvention = (CallingConvention) 2)]
    public static extern CBool SetWindowState(ConfigFlags flag);

    /// <summary>Clear window configuration state flags</summary>
    [DllImport("raylib", CallingConvention = (CallingConvention) 2)]
    public static extern void ClearWindowState(ConfigFlags flag);

    /// <summary>Toggle window state: fullscreen/windowed (only PLATFORM_DESKTOP)</summary>
    [DllImport("raylib", CallingConvention = (CallingConvention) 2)]
    public static extern void ToggleFullscreen();

    /// <summary>Set window state: maximized, if resizable (only PLATFORM_DESKTOP)</summary>
    [DllImport("raylib", CallingConvention = (CallingConvention) 2)]
    public static extern void MaximizeWindow();

    /// <summary>Set window state: minimized, if resizable (only PLATFORM_DESKTOP)</summary>
    [DllImport("raylib", CallingConvention = (CallingConvention) 2)]
    public static extern void MinimizeWindow();

    /// <summary>Set window state: not minimized/maximized (only PLATFORM_DESKTOP)</summary>
    [DllImport("raylib", CallingConvention = (CallingConvention) 2)]
    public static extern void RestoreWindow();

    /// <summary>Set icon for window (single image, RGBA 32bit, only PLATFORM_DESKTOP)</summary>
    [DllImport("raylib", CallingConvention = (CallingConvention) 2)]
    public static extern void SetWindowIcon(Image image);

    /// <summary>Set icon for window (multiple images, RGBA 32bit, only PLATFORM_DESKTOP)</summary>
    [DllImport("raylib", CallingConvention = (CallingConvention) 2)]
    public static extern unsafe void SetWindowIcons(Image* images, int count);

    /// <summary>Set title for window (only PLATFORM_DESKTOP)</summary>
    [DllImport("raylib", CallingConvention = (CallingConvention) 2)]
    public static extern unsafe void SetWindowTitle(sbyte* title);

    /// <summary>Set window position on screen (only PLATFORM_DESKTOP)</summary>
    [DllImport("raylib", CallingConvention = (CallingConvention) 2)]
    public static extern void SetWindowPosition(int x, int y);

    /// <summary>Set monitor for the current window (fullscreen mode)</summary>
    [DllImport("raylib", CallingConvention = (CallingConvention) 2)]
    public static extern void SetWindowMonitor(int monitor);

    /// <summary>Set window minimum dimensions (for FLAG_WINDOW_RESIZABLE)</summary>
    [DllImport("raylib", CallingConvention = (CallingConvention) 2)]
    public static extern void SetWindowMinSize(int width, int height);

    /// <summary>Set window dimensions</summary>
    [DllImport("raylib", CallingConvention = (CallingConvention) 2)]
    public static extern void SetWindowSize(int width, int height);

    /// <summary>Set window opacity [0.0f..1.0f] (only PLATFORM_DESKTOP)</summary>
    [DllImport("raylib", CallingConvention = (CallingConvention) 2)]
    public static extern void SetWindowOpacity(float opacity);

    /// <summary>Get native window handle</summary>
    [DllImport("raylib", CallingConvention = (CallingConvention) 2)]
    public static extern unsafe void* GetWindowHandle();

    /// <summary>Get current screen width</summary>
    [DllImport("raylib", CallingConvention = (CallingConvention) 2)]
    public static extern int GetScreenWidth();

    /// <summary>Get current screen height</summary>
    [DllImport("raylib", CallingConvention = (CallingConvention) 2)]
    public static extern int GetScreenHeight();

    /// <summary>Get current render width (it considers HiDPI)</summary>
    [DllImport("raylib", CallingConvention = (CallingConvention) 2)]
    public static extern int GetRenderWidth();

    /// <summary>Get current render height (it considers HiDPI)</summary>
    [DllImport("raylib", CallingConvention = (CallingConvention) 2)]
    public static extern int GetRenderHeight();

    /// <summary>Get number of connected monitors</summary>
    [DllImport("raylib", CallingConvention = (CallingConvention) 2)]
    public static extern int GetMonitorCount();

    /// <summary>Get current connected monitor</summary>
    [DllImport("raylib", CallingConvention = (CallingConvention) 2)]
    public static extern int GetCurrentMonitor();

    /// <summary>Get specified monitor position</summary>
    [DllImport("raylib", CallingConvention = (CallingConvention) 2)]
    public static extern Vector2 GetMonitorPosition(int monitor);

    /// <summary>Get specified monitor width</summary>
    [DllImport("raylib", CallingConvention = (CallingConvention) 2)]
    public static extern int GetMonitorWidth(int monitor);

    /// <summary>Get specified monitor height</summary>
    [DllImport("raylib", CallingConvention = (CallingConvention) 2)]
    public static extern int GetMonitorHeight(int monitor);

    /// <summary>Get specified monitor physical width in millimetres</summary>
    [DllImport("raylib", CallingConvention = (CallingConvention) 2)]
    public static extern int GetMonitorPhysicalWidth(int monitor);

    /// <summary>Get specified monitor physical height in millimetres</summary>
    [DllImport("raylib", CallingConvention = (CallingConvention) 2)]
    public static extern int GetMonitorPhysicalHeight(int monitor);

    /// <summary>Get specified monitor refresh rate</summary>
    [DllImport("raylib", CallingConvention = (CallingConvention) 2)]
    public static extern int GetMonitorRefreshRate(int monitor);

    /// <summary>Get window position XY on monitor</summary>
    [DllImport("raylib", CallingConvention = (CallingConvention) 2)]
    public static extern Vector2 GetWindowPosition();

    /// <summary>Get window scale DPI factor</summary>
    [DllImport("raylib", CallingConvention = (CallingConvention) 2)]
    public static extern Vector2 GetWindowScaleDPI();

    /// <summary>Get the human-readable, UTF-8 encoded name of the specified monitor</summary>
    [DllImport("raylib", CallingConvention = (CallingConvention) 2)]
    public static extern unsafe sbyte* GetMonitorName(int monitor);

    /// <summary>Get clipboard text content</summary>
    [DllImport("raylib", CallingConvention = (CallingConvention) 2)]
    public static extern unsafe sbyte* GetClipboardText();

    /// <summary>Set clipboard text content</summary>
    [DllImport("raylib", CallingConvention = (CallingConvention) 2)]
    public static extern unsafe void SetClipboardText(sbyte* text);

    /// <summary>Enable waiting for events on EndDrawing(), no automatic event polling</summary>
    [DllImport("raylib", CallingConvention = (CallingConvention) 2)]
    public static extern void EnableEventWaiting();

    /// <summary>Disable waiting for events on EndDrawing(), automatic events polling</summary>
    [DllImport("raylib", CallingConvention = (CallingConvention) 2)]
    public static extern void DisableEventWaiting();

    /// <summary>Swap back buffer with front buffer (screen drawing)</summary>
    [DllImport("raylib", CallingConvention = (CallingConvention) 2)]
    public static extern void SwapScreenBuffer();

    /// <summary>Register all input events</summary>
    [DllImport("raylib", CallingConvention = (CallingConvention) 2)]
    public static extern void PollInputEvents();

    /// <summary>Wait for some time (halt program execution)</summary>
    [DllImport("raylib", CallingConvention = (CallingConvention) 2)]
    public static extern void WaitTime(double seconds);

    /// <summary>Shows cursor</summary>
    [DllImport("raylib", CallingConvention = (CallingConvention) 2)]
    public static extern void ShowCursor();

    /// <summary>Hides cursor</summary>
    [DllImport("raylib", CallingConvention = (CallingConvention) 2)]
    public static extern void HideCursor();

    /// <summary>Check if cursor is not visible</summary>
    [DllImport("raylib", CallingConvention = (CallingConvention) 2)]
    public static extern CBool IsCursorHidden();

    /// <summary>Enables cursor (unlock cursor)</summary>
    [DllImport("raylib", CallingConvention = (CallingConvention) 2)]
    public static extern void EnableCursor();

    /// <summary>Disables cursor (lock cursor)</summary>
    [DllImport("raylib", CallingConvention = (CallingConvention) 2)]
    public static extern void DisableCursor();

    /// <summary>Check if cursor is on the screen</summary>
    [DllImport("raylib", CallingConvention = (CallingConvention) 2)]
    public static extern CBool IsCursorOnScreen();

    /// <summary>Set background color (framebuffer clear color)</summary>
    [DllImport("raylib", CallingConvention = (CallingConvention) 2)]
    public static extern void ClearBackground(Color color);

    /// <summary>Setup canvas (framebuffer) to start drawing</summary>
    [DllImport("raylib", CallingConvention = (CallingConvention) 2)]
    public static extern void BeginDrawing();

    /// <summary>End canvas drawing and swap buffers (double buffering)</summary>
    [DllImport("raylib", CallingConvention = (CallingConvention) 2)]
    public static extern void EndDrawing();

    /// <summary>Initialize 2D mode with custom camera (2D)</summary>
    [DllImport("raylib", CallingConvention = (CallingConvention) 2)]
    public static extern void BeginMode2D(Camera2D camera);

    /// <summary>Ends 2D mode with custom camera</summary>
    [DllImport("raylib", CallingConvention = (CallingConvention) 2)]
    public static extern void EndMode2D();

    /// <summary>Initializes 3D mode with custom camera (3D)</summary>
    [DllImport("raylib", CallingConvention = (CallingConvention) 2)]
    public static extern void BeginMode3D(Camera3D camera);

    /// <summary>Ends 3D mode and returns to default 2D orthographic mode</summary>
    [DllImport("raylib", CallingConvention = (CallingConvention) 2)]
    public static extern void EndMode3D();

    /// <summary>Initializes render texture for drawing</summary>
    [DllImport("raylib", CallingConvention = (CallingConvention) 2)]
    public static extern void BeginTextureMode(RenderTexture2D target);

    /// <summary>Ends drawing to render texture</summary>
    [DllImport("raylib", CallingConvention = (CallingConvention) 2)]
    public static extern void EndTextureMode();

    /// <summary>Begin custom shader drawing</summary>
    [DllImport("raylib", CallingConvention = (CallingConvention) 2)]
    public static extern void BeginShaderMode(Shader shader);

    /// <summary>End custom shader drawing (use default shader)</summary>
    [DllImport("raylib", CallingConvention = (CallingConvention) 2)]
    public static extern void EndShaderMode();

    /// <summary>Begin blending mode (alpha, additive, multiplied)</summary>
    [DllImport("raylib", CallingConvention = (CallingConvention) 2)]
    public static extern void BeginBlendMode(BlendMode mode);

    /// <summary>End blending mode (reset to default: alpha blending)</summary>
    [DllImport("raylib", CallingConvention = (CallingConvention) 2)]
    public static extern void EndBlendMode();

    /// <summary>Begin scissor mode (define screen area for following drawing)</summary>
    [DllImport("raylib", CallingConvention = (CallingConvention) 2)]
    public static extern void BeginScissorMode(int x, int y, int width, int height);

    /// <summary>End scissor mode</summary>
    [DllImport("raylib", CallingConvention = (CallingConvention) 2)]
    public static extern void EndScissorMode();

    /// <summary>Begin stereo rendering (requires VR simulator)</summary>
    [DllImport("raylib", CallingConvention = (CallingConvention) 2)]
    public static extern void BeginVrStereoMode(VrStereoConfig config);

    /// <summary>End stereo rendering (requires VR simulator)</summary>
    [DllImport("raylib", CallingConvention = (CallingConvention) 2)]
    public static extern void EndVrStereoMode();

    /// <summary>Load VR stereo config for VR simulator device parameters</summary>
    [DllImport("raylib", CallingConvention = (CallingConvention) 2)]
    public static extern VrStereoConfig LoadVrStereoConfig(VrDeviceInfo device);

    /// <summary>Unload VR stereo configs</summary>
    [DllImport("raylib", CallingConvention = (CallingConvention) 2)]
    public static extern void UnloadVrStereoConfig(VrStereoConfig config);

    /// <summary>Load shader from files and bind default locations</summary>
    [DllImport("raylib", CallingConvention = (CallingConvention) 2)]
    public static extern unsafe Shader LoadShader(sbyte* vsFileName, sbyte* fsFileName);

    /// <summary>Load shader from code strings and bind default locations</summary>
    [DllImport("raylib", CallingConvention = (CallingConvention) 2)]
    public static extern unsafe Shader LoadShaderFromMemory(sbyte* vsCode, sbyte* fsCode);

    /// <summary>Check if a shader is ready</summary>
    [DllImport("raylib", CallingConvention = (CallingConvention) 2)]
    public static extern CBool IsShaderReady(Shader shader);

    /// <summary>Get shader uniform location</summary>
    [DllImport("raylib", CallingConvention = (CallingConvention) 2)]
    public static extern unsafe int GetShaderLocation(Shader shader, sbyte* uniformName);

    /// <summary>Get shader attribute location</summary>
    [DllImport("raylib", CallingConvention = (CallingConvention) 2)]
    public static extern unsafe int GetShaderLocationAttrib(Shader shader, sbyte* attribName);

    /// <summary>Set shader uniform value</summary>
    [DllImport("raylib", CallingConvention = (CallingConvention) 2)]
    public static extern unsafe void SetShaderValue(
      Shader shader,
      int locIndex,
      void* value,
      ShaderUniformDataType uniformType);

    /// <summary>Set shader uniform value vector</summary>
    [DllImport("raylib", CallingConvention = (CallingConvention) 2)]
    public static extern unsafe void SetShaderValueV(
      Shader shader,
      int locIndex,
      void* value,
      ShaderUniformDataType uniformType,
      int count);

    /// <summary>Set shader uniform value (matrix 4x4)</summary>
    [DllImport("raylib", CallingConvention = (CallingConvention) 2)]
    public static extern void SetShaderValueMatrix(Shader shader, int locIndex, Matrix4x4 mat);

    /// <summary>Set shader uniform value for texture (sampler2d)</summary>
    [DllImport("raylib", CallingConvention = (CallingConvention) 2)]
    public static extern void SetShaderValueTexture(Shader shader, int locIndex, Texture2D texture);

    /// <summary>Unload shader from GPU memory (VRAM)</summary>
    [DllImport("raylib", CallingConvention = (CallingConvention) 2)]
    public static extern void UnloadShader(Shader shader);

    /// <summary>Get a ray trace from mouse position</summary>
    [DllImport("raylib", CallingConvention = (CallingConvention) 2)]
    public static extern Ray GetMouseRay(Vector2 mousePosition, Camera3D camera);

    /// <summary>Get camera transform matrix (view matrix)</summary>
    [DllImport("raylib", CallingConvention = (CallingConvention) 2)]
    public static extern Matrix4x4 GetCameraMatrix(Camera3D camera);

    /// <summary>Get camera 2d transform matrix</summary>
    [DllImport("raylib", CallingConvention = (CallingConvention) 2)]
    public static extern Matrix4x4 GetCameraMatrix2D(Camera2D camera);

    /// <summary>Get the screen space position for a 3d world space position</summary>
    [DllImport("raylib", CallingConvention = (CallingConvention) 2)]
    public static extern Vector2 GetWorldToScreen(Vector3 position, Camera3D camera);

    /// <summary>Get size position for a 3d world space position</summary>
    [DllImport("raylib", CallingConvention = (CallingConvention) 2)]
    public static extern Vector2 GetWorldToScreenEx(
      Vector3 position,
      Camera3D camera,
      int width,
      int height);

    /// <summary>Get the screen space position for a 2d camera world space position</summary>
    [DllImport("raylib", CallingConvention = (CallingConvention) 2)]
    public static extern Vector2 GetWorldToScreen2D(Vector2 position, Camera2D camera);

    /// <summary>Get the world space position for a 2d camera screen space position</summary>
    [DllImport("raylib", CallingConvention = (CallingConvention) 2)]
    public static extern Vector2 GetScreenToWorld2D(Vector2 position, Camera2D camera);

    /// <summary>Set target FPS (maximum)</summary>
    [DllImport("raylib", CallingConvention = (CallingConvention) 2)]
    public static extern void SetTargetFPS(int fps);

    /// <summary>Get current FPS</summary>
    [DllImport("raylib", CallingConvention = (CallingConvention) 2)]
    public static extern int GetFPS();

    /// <summary>Get time in seconds for last frame drawn</summary>
    [DllImport("raylib", CallingConvention = (CallingConvention) 2)]
    public static extern float GetFrameTime();

    /// <summary>Get elapsed time in seconds since InitWindow()</summary>
    [DllImport("raylib", CallingConvention = (CallingConvention) 2)]
    public static extern double GetTime();

    /// <summary>Get a random value between min and max (both included)</summary>
    [DllImport("raylib", CallingConvention = (CallingConvention) 2)]
    public static extern int GetRandomValue(int min, int max);

    /// <summary>Set the seed for the random number generator</summary>
    [DllImport("raylib", CallingConvention = (CallingConvention) 2)]
    public static extern int SetRandomSeed(uint seed);

    /// <summary>Takes a screenshot of current screen (saved a .png)</summary>
    [DllImport("raylib", CallingConvention = (CallingConvention) 2)]
    public static extern unsafe void TakeScreenshot(sbyte* fileName);

    /// <summary>Setup window configuration flags (view FLAGS)</summary>
    [DllImport("raylib", CallingConvention = (CallingConvention) 2)]
    public static extern void SetConfigFlags(ConfigFlags flags);

    /// <summary>Show trace log messages (LOG_DEBUG, LOG_INFO, LOG_WARNING, LOG_ERROR)</summary>
    [DllImport("raylib", CallingConvention = (CallingConvention) 2)]
    public static extern unsafe void TraceLog(TraceLogLevel logLevel, sbyte* text);

    /// <summary>Set the current threshold (minimum) log level</summary>
    [DllImport("raylib", CallingConvention = (CallingConvention) 2)]
    public static extern void SetTraceLogLevel(TraceLogLevel logLevel);

    /// <summary>Internal memory allocator</summary>
    [DllImport("raylib", CallingConvention = (CallingConvention) 2)]
    public static extern unsafe void* MemAlloc(int size);

    /// <summary>Internal memory reallocator</summary>
    [DllImport("raylib", CallingConvention = (CallingConvention) 2)]
    public static extern unsafe void* MemRealloc(void* ptr, int size);

    /// <summary>Internal memory free</summary>
    [DllImport("raylib", CallingConvention = (CallingConvention) 2)]
    public static extern unsafe void MemFree(void* ptr);

    /// <summary>Set custom trace log</summary>
    [DllImport("raylib", CallingConvention = (CallingConvention) 2)]
    public static extern void SetTraceLogCallback(__FnPtr<void (int, sbyte*, sbyte*)> callback);

    /// <summary>Set custom file binary data loader</summary>
    [DllImport("raylib", CallingConvention = (CallingConvention) 2)]
    public static extern void SetLoadFileDataCallback(__FnPtr<byte* (sbyte*, uint*)> callback);

    /// <summary>Set custom file binary data saver</summary>
    [DllImport("raylib", CallingConvention = (CallingConvention) 2)]
    public static extern void SetSaveFileDataCallback(__FnPtr<CBool (sbyte*, void*, uint)> callback);

    /// <summary>Set custom file text data loader</summary>
    [DllImport("raylib", CallingConvention = (CallingConvention) 2)]
    public static extern void SetLoadFileTextCallback(__FnPtr<sbyte* (sbyte*)> callback);

    /// <summary>Set custom file text data saver</summary>
    [DllImport("raylib", CallingConvention = (CallingConvention) 2)]
    public static extern void SetSaveFileTextCallback(__FnPtr<CBool (sbyte*, sbyte*)> callback);

    /// <summary>Load file data as byte array (read)</summary>
    [DllImport("raylib", CallingConvention = (CallingConvention) 2)]
    public static extern unsafe byte* LoadFileData(sbyte* fileName, uint* bytesRead);

    /// <summary>Unload file data allocated by LoadFileData()</summary>
    [DllImport("raylib", CallingConvention = (CallingConvention) 2)]
    public static extern unsafe void UnloadFileData(byte* data);

    /// <summary>Save data to file from byte array (write), returns true on success</summary>
    [DllImport("raylib", CallingConvention = (CallingConvention) 2)]
    public static extern unsafe CBool SaveFileData(sbyte* fileName, void* data, uint bytesToWrite);

    /// <summary>Export data to code (.h), returns true on success</summary>
    [DllImport("raylib", CallingConvention = (CallingConvention) 2)]
    public static extern unsafe CBool ExportDataAsCode(byte* data, uint size, sbyte* fileName);

    [DllImport("raylib", CallingConvention = (CallingConvention) 2)]
    public static extern unsafe char* LoadFileText(sbyte* fileName);

    [DllImport("raylib", CallingConvention = (CallingConvention) 2)]
    public static extern unsafe void UnloadFileText(char* text);

    [DllImport("raylib", CallingConvention = (CallingConvention) 2)]
    public static extern unsafe CBool SaveFileText(sbyte* fileName, char* text);

    [DllImport("raylib", CallingConvention = (CallingConvention) 2)]
    public static extern unsafe CBool FileExists(sbyte* fileName);

    [DllImport("raylib", CallingConvention = (CallingConvention) 2)]
    public static extern unsafe CBool DirectoryExists(sbyte* dirPath);

    /// <summary>Check file extension (including point: .png, .wav)</summary>
    [DllImport("raylib", CallingConvention = (CallingConvention) 2)]
    public static extern unsafe CBool IsFileExtension(sbyte* fileName, sbyte* ext);

    /// <summary>Get file length in bytes</summary>
    [DllImport("raylib", CallingConvention = (CallingConvention) 2)]
    public static extern unsafe int GetFileLength(sbyte* fileName);

    /// <summary>Get pointer to extension for a filename string (includes dot: '.png')</summary>
    [DllImport("raylib", CallingConvention = (CallingConvention) 2)]
    public static extern unsafe sbyte* GetFileExtension(sbyte* fileName);

    /// <summary>Get pointer to filename for a path string</summary>
    [DllImport("raylib", CallingConvention = (CallingConvention) 2)]
    public static extern unsafe sbyte* GetFileName(sbyte* filePath);

    /// <summary>Get filename string without extension (uses static string)</summary>
    [DllImport("raylib", CallingConvention = (CallingConvention) 2)]
    public static extern unsafe sbyte* GetFileNameWithoutExt(sbyte* filePath);

    /// <summary>Get full path for a given fileName with path (uses static string)</summary>
    [DllImport("raylib", CallingConvention = (CallingConvention) 2)]
    public static extern unsafe sbyte* GetDirectoryPath(sbyte* filePath);

    /// <summary>Get previous directory path for a given path (uses static string)</summary>
    [DllImport("raylib", CallingConvention = (CallingConvention) 2)]
    public static extern unsafe sbyte* GetPrevDirectoryPath(sbyte* dirPath);

    /// <summary>Get current working directory (uses static string)</summary>
    [DllImport("raylib", CallingConvention = (CallingConvention) 2)]
    public static extern unsafe sbyte* GetWorkingDirectory();

    /// <summary>Get the directory of the running application (uses static string)</summary>
    [DllImport("raylib", CallingConvention = (CallingConvention) 2)]
    public static extern unsafe sbyte* GetApplicationDirectory();

    /// <summary>Load directory filepaths</summary>
    [DllImport("raylib", CallingConvention = (CallingConvention) 2)]
    public static extern unsafe FilePathList LoadDirectoryFiles(sbyte* dirPath, int* count);

    /// <summary>Load directory filepaths with extension filtering and recursive directory scan</summary>
    [DllImport("raylib", CallingConvention = (CallingConvention) 2)]
    public static extern unsafe FilePathList LoadDirectoryFilesEx(
      sbyte* basePath,
      sbyte* filter,
      CBool scanSubdirs);

    /// <summary>Unload filepaths</summary>
    [DllImport("raylib", CallingConvention = (CallingConvention) 2)]
    public static extern void UnloadDirectoryFiles(FilePathList files);

    /// <summary>Check if a given path is a file or a directory</summary>
    [DllImport("raylib", CallingConvention = (CallingConvention) 2)]
    public static extern unsafe CBool IsPathFile(sbyte* path);

    /// <summary>Change working directory, return true on success</summary>
    [DllImport("raylib", CallingConvention = (CallingConvention) 2)]
    public static extern unsafe CBool ChangeDirectory(sbyte* dir);

    /// <summary>Check if a file has been dropped into window</summary>
    [DllImport("raylib", CallingConvention = (CallingConvention) 2)]
    public static extern CBool IsFileDropped();

    /// <summary>Load dropped filepaths</summary>
    [DllImport("raylib", CallingConvention = (CallingConvention) 2)]
    public static extern FilePathList LoadDroppedFiles();

    /// <summary>Unload dropped filepaths</summary>
    [DllImport("raylib", CallingConvention = (CallingConvention) 2)]
    public static extern void UnloadDroppedFiles(FilePathList files);

    /// <summary>Get file modification time (last write time)</summary>
    [DllImport("raylib", CallingConvention = (CallingConvention) 2)]
    public static extern unsafe int GetFileModTime(sbyte* fileName);

    /// <summary>Compress data (DEFLATE algorithm), memory must be MemFree()</summary>
    [DllImport("raylib", CallingConvention = (CallingConvention) 2)]
    public static extern unsafe byte* CompressData(byte* data, int dataSize, int* compDataSize);

    /// <summary>Decompress data (DEFLATE algorithm), memory must be MemFree()</summary>
    [DllImport("raylib", CallingConvention = (CallingConvention) 2)]
    public static extern unsafe byte* DecompressData(
      byte* compData,
      int compDataSize,
      int* dataSize);

    /// <summary>Encode data to Base64 string, memory must be MemFree()</summary>
    [DllImport("raylib", CallingConvention = (CallingConvention) 2)]
    public static extern unsafe sbyte* EncodeDataBase64(byte* data, int dataSize, int* outputSize);

    /// <summary>Decode Base64 string data, memory must be MemFree()</summary>
    [DllImport("raylib", CallingConvention = (CallingConvention) 2)]
    public static extern unsafe byte* DecodeDataBase64(byte* data, int* outputSize);

    /// <summary>Open URL with default system browser (if available)</summary>
    [DllImport("raylib", CallingConvention = (CallingConvention) 2)]
    public static extern unsafe void OpenURL(sbyte* url);

    /// <summary>Detect if a key has been pressed once</summary>
    [DllImport("raylib", CallingConvention = (CallingConvention) 2)]
    public static extern CBool IsKeyPressed(KeyboardKey key);

    /// <summary>Detect if a key is being pressed</summary>
    [DllImport("raylib", CallingConvention = (CallingConvention) 2)]
    public static extern CBool IsKeyDown(KeyboardKey key);

    /// <summary>Detect if a key has been released once</summary>
    [DllImport("raylib", CallingConvention = (CallingConvention) 2)]
    public static extern CBool IsKeyReleased(KeyboardKey key);

    /// <summary>Detect if a key is NOT being pressed</summary>
    [DllImport("raylib", CallingConvention = (CallingConvention) 2)]
    public static extern CBool IsKeyUp(KeyboardKey key);

    /// <summary>Set a custom key to exit program (default is ESC)</summary>
    [DllImport("raylib", CallingConvention = (CallingConvention) 2)]
    public static extern void SetExitKey(KeyboardKey key);

    /// <summary>
    /// Get key pressed (keycode), call it multiple times for keys queued, returns 0 when the queue is empty
    /// </summary>
    [DllImport("raylib", CallingConvention = (CallingConvention) 2)]
    public static extern int GetKeyPressed();

    /// <summary>
    /// Get char pressed (unicode), call it multiple times for chars queued, returns 0 when the queue is empty
    /// </summary>
    [DllImport("raylib", CallingConvention = (CallingConvention) 2)]
    public static extern int GetCharPressed();

    /// <summary>Detect if a gamepad is available</summary>
    [DllImport("raylib", CallingConvention = (CallingConvention) 2)]
    public static extern CBool IsGamepadAvailable(int gamepad);

    /// <summary>Get gamepad internal name id</summary>
    [DllImport("raylib", CallingConvention = (CallingConvention) 2)]
    public static extern unsafe sbyte* GetGamepadName(int gamepad);

    /// <summary>Detect if a gamepad button has been pressed once</summary>
    [DllImport("raylib", CallingConvention = (CallingConvention) 2)]
    public static extern CBool IsGamepadButtonPressed(int gamepad, GamepadButton button);

    /// <summary>Detect if a gamepad button is being pressed</summary>
    [DllImport("raylib", CallingConvention = (CallingConvention) 2)]
    public static extern CBool IsGamepadButtonDown(int gamepad, GamepadButton button);

    /// <summary>Detect if a gamepad button has been released once</summary>
    [DllImport("raylib", CallingConvention = (CallingConvention) 2)]
    public static extern CBool IsGamepadButtonReleased(int gamepad, GamepadButton button);

    /// <summary>Detect if a gamepad button is NOT being pressed</summary>
    [DllImport("raylib", CallingConvention = (CallingConvention) 2)]
    public static extern CBool IsGamepadButtonUp(int gamepad, GamepadButton button);

    /// <summary>Get the last gamepad button pressed</summary>
    [DllImport("raylib", CallingConvention = (CallingConvention) 2)]
    public static extern int GetGamepadButtonPressed();

    /// <summary>Get gamepad axis count for a gamepad</summary>
    [DllImport("raylib", CallingConvention = (CallingConvention) 2)]
    public static extern int GetGamepadAxisCount(int gamepad);

    /// <summary>Get axis movement value for a gamepad axis</summary>
    [DllImport("raylib", CallingConvention = (CallingConvention) 2)]
    public static extern float GetGamepadAxisMovement(int gamepad, GamepadAxis axis);

    /// <summary>Set internal gamepad mappings (SDL_GameControllerDB)</summary>
    [DllImport("raylib", CallingConvention = (CallingConvention) 2)]
    public static extern unsafe int SetGamepadMappings(sbyte* mappings);

    /// <summary>Detect if a mouse button has been pressed once</summary>
    [DllImport("raylib", CallingConvention = (CallingConvention) 2)]
    public static extern CBool IsMouseButtonPressed(MouseButton button);

    /// <summary>Detect if a mouse button is being pressed</summary>
    [DllImport("raylib", CallingConvention = (CallingConvention) 2)]
    public static extern CBool IsMouseButtonDown(MouseButton button);

    /// <summary>Detect if a mouse button has been released once</summary>
    [DllImport("raylib", CallingConvention = (CallingConvention) 2)]
    public static extern CBool IsMouseButtonReleased(MouseButton button);

    /// <summary>Detect if a mouse button is NOT being pressed</summary>
    [DllImport("raylib", CallingConvention = (CallingConvention) 2)]
    public static extern CBool IsMouseButtonUp(MouseButton button);

    /// <summary>Get mouse position X</summary>
    [DllImport("raylib", CallingConvention = (CallingConvention) 2)]
    public static extern int GetMouseX();

    /// <summary>Get mouse position Y</summary>
    [DllImport("raylib", CallingConvention = (CallingConvention) 2)]
    public static extern int GetMouseY();

    /// <summary>Get mouse position XY</summary>
    [DllImport("raylib", CallingConvention = (CallingConvention) 2)]
    public static extern Vector2 GetMousePosition();

    /// <summary>Get mouse delta between frames</summary>
    [DllImport("raylib", CallingConvention = (CallingConvention) 2)]
    public static extern Vector2 GetMouseDelta();

    /// <summary>Set mouse position XY</summary>
    [DllImport("raylib", CallingConvention = (CallingConvention) 2)]
    public static extern void SetMousePosition(int x, int y);

    /// <summary>Set mouse offset</summary>
    [DllImport("raylib", CallingConvention = (CallingConvention) 2)]
    public static extern void SetMouseOffset(int offsetX, int offsetY);

    /// <summary>Set mouse scaling</summary>
    [DllImport("raylib", CallingConvention = (CallingConvention) 2)]
    public static extern void SetMouseScale(float scaleX, float scaleY);

    /// <summary>Get mouse wheel movement for X or Y, whichever is larger</summary>
    [DllImport("raylib", CallingConvention = (CallingConvention) 2)]
    public static extern float GetMouseWheelMove();

    /// <summary>Get mouse wheel movement for both X and Y</summary>
    [DllImport("raylib", CallingConvention = (CallingConvention) 2)]
    public static extern Vector2 GetMouseWheelMoveV();

    /// <summary>Set mouse cursor</summary>
    [DllImport("raylib", CallingConvention = (CallingConvention) 2)]
    public static extern void SetMouseCursor(MouseCursor cursor);

    /// <summary>Get touch position X for touch point 0 (relative to screen size)</summary>
    [DllImport("raylib", CallingConvention = (CallingConvention) 2)]
    public static extern int GetTouchX();

    /// <summary>Get touch position Y for touch point 0 (relative to screen size)</summary>
    [DllImport("raylib", CallingConvention = (CallingConvention) 2)]
    public static extern int GetTouchY();

    /// <summary>Get touch position XY for a touch point index (relative to screen size)</summary>
    [DllImport("raylib", CallingConvention = (CallingConvention) 2)]
    public static extern Vector2 GetTouchPosition(int index);

    /// <summary>Get touch point identifier for given index</summary>
    [DllImport("raylib", CallingConvention = (CallingConvention) 2)]
    public static extern int GetTouchPointId(int index);

    /// <summary>Get number of touch points</summary>
    [DllImport("raylib", CallingConvention = (CallingConvention) 2)]
    public static extern int GetTouchPointCount();

    /// <summary>Enable a set of gestures using flags</summary>
    [DllImport("raylib", CallingConvention = (CallingConvention) 2)]
    public static extern void SetGesturesEnabled(Gesture flags);

    /// <summary>Check if a gesture has been detected</summary>
    [DllImport("raylib", CallingConvention = (CallingConvention) 2)]
    public static extern CBool IsGestureDetected(Gesture gesture);

    /// <summary>Get latest detected gesture</summary>
    [DllImport("raylib", CallingConvention = (CallingConvention) 2)]
    public static extern Gesture GetGestureDetected();

    /// <summary>Get gesture hold time in milliseconds</summary>
    [DllImport("raylib", CallingConvention = (CallingConvention) 2)]
    public static extern float GetGestureHoldDuration();

    /// <summary>Get gesture drag vector</summary>
    [DllImport("raylib", CallingConvention = (CallingConvention) 2)]
    public static extern Vector2 GetGestureDragVector();

    /// <summary>Get gesture drag angle</summary>
    [DllImport("raylib", CallingConvention = (CallingConvention) 2)]
    public static extern float GetGestureDragAngle();

    /// <summary>Get gesture pinch delta</summary>
    [DllImport("raylib", CallingConvention = (CallingConvention) 2)]
    public static extern Vector2 GetGesturePinchVector();

    /// <summary>Get gesture pinch angle</summary>
    [DllImport("raylib", CallingConvention = (CallingConvention) 2)]
    public static extern float GetGesturePinchAngle();

    /// <summary>Update camera position for selected mode</summary>
    [DllImport("raylib", CallingConvention = (CallingConvention) 2)]
    public static extern unsafe void UpdateCamera(Camera3D* camera, CameraMode mode);

    /// <summary>Update camera movement/rotation</summary>
    [DllImport("raylib", CallingConvention = (CallingConvention) 2)]
    public static extern unsafe void UpdateCameraPro(
      Camera3D* camera,
      Vector3 movement,
      Vector3 rotation,
      float zoom);

    /// <summary>Returns the cameras forward vector (normalized)</summary>
    [DllImport("raylib", CallingConvention = (CallingConvention) 2)]
    public static extern unsafe Vector3 GetCameraForward(Camera3D* camera);

    /// <summary>
    /// Returns the cameras up vector (normalized)<br />
    /// NOTE: The up vector might not be perpendicular to the forward vector
    /// </summary>
    [DllImport("raylib", CallingConvention = (CallingConvention) 2)]
    public static extern unsafe Vector3 GetCameraUp(Camera3D* camera);

    /// <summary>Returns the cameras right vector (normalized)</summary>
    [DllImport("raylib", CallingConvention = (CallingConvention) 2)]
    public static extern unsafe Vector3 GetCameraRight(Camera3D* camera);

    /// <summary>Moves the camera in its forward direction</summary>
    [DllImport("raylib", CallingConvention = (CallingConvention) 2)]
    public static extern unsafe void CameraMoveForward(
      Camera3D* camera,
      float distance,
      CBool moveInWorldPlane);

    /// <summary>Moves the camera in its up direction</summary>
    [DllImport("raylib", CallingConvention = (CallingConvention) 2)]
    public static extern unsafe void CameraMoveUp(Camera3D* camera, float distance);

    /// <summary>Moves the camera target in its current right direction</summary>
    [DllImport("raylib", CallingConvention = (CallingConvention) 2)]
    public static extern unsafe void CameraMoveRight(
      Camera3D* camera,
      float distance,
      CBool moveInWorldPlane);

    /// <summary>Moves the camera position closer/farther to/from the camera target</summary>
    [DllImport("raylib", CallingConvention = (CallingConvention) 2)]
    public static extern unsafe void CameraMoveToTarget(Camera3D* camera, float delta);

    /// <summary>
    /// Rotates the camera around its up vector<br />
    /// If rotateAroundTarget is false, the camera rotates around its position
    /// </summary>
    [DllImport("raylib", CallingConvention = (CallingConvention) 2)]
    public static extern unsafe void CameraYaw(
      Camera3D* camera,
      float angle,
      CBool rotateAroundTarget);

    /// <summary>Rotates the camera around its right vector</summary>
    [DllImport("raylib", CallingConvention = (CallingConvention) 2)]
    public static extern unsafe void CameraPitch(
      Camera3D* camera,
      float angle,
      CBool lockView,
      CBool rotateAroundTarget,
      CBool rotateUp);

    /// <summary>Rotates the camera around its forward vector</summary>
    [DllImport("raylib", CallingConvention = (CallingConvention) 2)]
    public static extern unsafe void CameraRoll(Camera3D* camera, float angle);

    /// <summary>Returns the camera view matrix</summary>
    [DllImport("raylib", CallingConvention = (CallingConvention) 2)]
    public static extern unsafe Matrix4x4 GetCameraViewMatrix(Camera3D* camera);

    /// <summary>Returns the camera projection matrix</summary>
    [DllImport("raylib", CallingConvention = (CallingConvention) 2)]
    public static extern unsafe Matrix4x4 GetCameraProjectionMatrix(Camera3D* camera, float aspect);

    /// <summary>
    /// Set texture and rectangle to be used on shapes drawing<br />
    /// NOTE: It can be useful when using basic shapes and one single font.<br />
    /// Defining a white rectangle would allow drawing everything in a single draw call.
    /// </summary>
    [DllImport("raylib", CallingConvention = (CallingConvention) 2)]
    public static extern void SetShapesTexture(Texture2D texture, Rectangle source);

    /// <summary>Draw a pixel</summary>
    [DllImport("raylib", CallingConvention = (CallingConvention) 2)]
    public static extern void DrawPixel(int posX, int posY, Color color);

    /// <summary>Draw a pixel (Vector version)</summary>
    [DllImport("raylib", CallingConvention = (CallingConvention) 2)]
    public static extern void DrawPixelV(Vector2 position, Color color);

    /// <summary>Draw a line</summary>
    [DllImport("raylib", CallingConvention = (CallingConvention) 2)]
    public static extern void DrawLine(
      int startPosX,
      int startPosY,
      int endPosX,
      int endPosY,
      Color color);

    /// <summary>Draw a line (Vector version)</summary>
    [DllImport("raylib", CallingConvention = (CallingConvention) 2)]
    public static extern void DrawLineV(Vector2 startPos, Vector2 endPos, Color color);

    /// <summary>Draw a line defining thickness</summary>
    [DllImport("raylib", CallingConvention = (CallingConvention) 2)]
    public static extern void DrawLineEx(
      Vector2 startPos,
      Vector2 endPos,
      float thick,
      Color color);

    /// <summary>Draw a line using cubic-bezier curves in-out</summary>
    [DllImport("raylib", CallingConvention = (CallingConvention) 2)]
    public static extern void DrawLineBezier(
      Vector2 startPos,
      Vector2 endPos,
      float thick,
      Color color);

    /// <summary>Draw line using quadratic bezier curves with a control point</summary>
    [DllImport("raylib", CallingConvention = (CallingConvention) 2)]
    public static extern void DrawLineBezierQuad(
      Vector2 startPos,
      Vector2 endPos,
      Vector2 controlPos,
      float thick,
      Color color);

    /// <summary>Draw line using cubic bezier curves with 2 control points</summary>
    [DllImport("raylib", CallingConvention = (CallingConvention) 2)]
    public static extern void DrawLineBezierCubic(
      Vector2 startPos,
      Vector2 endPos,
      Vector2 startControlPos,
      Vector2 endControlPos,
      float thick,
      Color color);

    /// <summary>Draw lines sequence</summary>
    [DllImport("raylib", CallingConvention = (CallingConvention) 2)]
    public static extern unsafe void DrawLineStrip(Vector2* points, int pointCount, Color color);

    /// <summary>Draw a color-filled circle</summary>
    [DllImport("raylib", CallingConvention = (CallingConvention) 2)]
    public static extern void DrawCircle(int centerX, int centerY, float radius, Color color);

    /// <summary>Draw a piece of a circle</summary>
    [DllImport("raylib", CallingConvention = (CallingConvention) 2)]
    public static extern void DrawCircleSector(
      Vector2 center,
      float radius,
      float startAngle,
      float endAngle,
      int segments,
      Color color);

    /// <summary>Draw circle sector outline</summary>
    [DllImport("raylib", CallingConvention = (CallingConvention) 2)]
    public static extern void DrawCircleSectorLines(
      Vector2 center,
      float radius,
      float startAngle,
      float endAngle,
      int segments,
      Color color);

    /// <summary>Draw a gradient-filled circle</summary>
    [DllImport("raylib", CallingConvention = (CallingConvention) 2)]
    public static extern void DrawCircleGradient(
      int centerX,
      int centerY,
      float radius,
      Color color1,
      Color color2);

    /// <summary>Draw a color-filled circle (Vector version)</summary>
    [DllImport("raylib", CallingConvention = (CallingConvention) 2)]
    public static extern void DrawCircleV(Vector2 center, float radius, Color color);

    /// <summary>Draw circle outline</summary>
    [DllImport("raylib", CallingConvention = (CallingConvention) 2)]
    public static extern void DrawCircleLines(int centerX, int centerY, float radius, Color color);

    /// <summary>Draw ellipse</summary>
    [DllImport("raylib", CallingConvention = (CallingConvention) 2)]
    public static extern void DrawEllipse(
      int centerX,
      int centerY,
      float radiusH,
      float radiusV,
      Color color);

    /// <summary>Draw ellipse outline</summary>
    [DllImport("raylib", CallingConvention = (CallingConvention) 2)]
    public static extern void DrawEllipseLines(
      int centerX,
      int centerY,
      float radiusH,
      float radiusV,
      Color color);

    /// <summary>Draw ring</summary>
    [DllImport("raylib", CallingConvention = (CallingConvention) 2)]
    public static extern void DrawRing(
      Vector2 center,
      float innerRadius,
      float outerRadius,
      float startAngle,
      float endAngle,
      int segments,
      Color color);

    /// <summary>Draw ring outline</summary>
    [DllImport("raylib", CallingConvention = (CallingConvention) 2)]
    public static extern void DrawRingLines(
      Vector2 center,
      float innerRadius,
      float outerRadius,
      float startAngle,
      float endAngle,
      int segments,
      Color color);

    /// <summary>Draw a color-filled rectangle</summary>
    [DllImport("raylib", CallingConvention = (CallingConvention) 2)]
    public static extern void DrawRectangle(
      int posX,
      int posY,
      int width,
      int height,
      Color color);

    /// <summary>Draw a color-filled rectangle (Vector version)</summary>
    [DllImport("raylib", CallingConvention = (CallingConvention) 2)]
    public static extern void DrawRectangleV(Vector2 position, Vector2 size, Color color);

    /// <summary>Draw a color-filled rectangle</summary>
    [DllImport("raylib", CallingConvention = (CallingConvention) 2)]
    public static extern void DrawRectangleRec(Rectangle rec, Color color);

    /// <summary>Draw a color-filled rectangle with pro parameters</summary>
    [DllImport("raylib", CallingConvention = (CallingConvention) 2)]
    public static extern void DrawRectanglePro(
      Rectangle rec,
      Vector2 origin,
      float rotation,
      Color color);

    /// <summary>Draw a vertical-gradient-filled rectangle</summary>
    [DllImport("raylib", CallingConvention = (CallingConvention) 2)]
    public static extern void DrawRectangleGradientV(
      int posX,
      int posY,
      int width,
      int height,
      Color color1,
      Color color2);

    /// <summary>Draw a horizontal-gradient-filled rectangle</summary>
    [DllImport("raylib", CallingConvention = (CallingConvention) 2)]
    public static extern void DrawRectangleGradientH(
      int posX,
      int posY,
      int width,
      int height,
      Color color1,
      Color color2);

    /// <summary>Draw a gradient-filled rectangle with custom vertex colors</summary>
    [DllImport("raylib", CallingConvention = (CallingConvention) 2)]
    public static extern void DrawRectangleGradientEx(
      Rectangle rec,
      Color col1,
      Color col2,
      Color col3,
      Color col4);

    /// <summary>Draw rectangle outline</summary>
    [DllImport("raylib", CallingConvention = (CallingConvention) 2)]
    public static extern void DrawRectangleLines(
      int posX,
      int posY,
      int width,
      int height,
      Color color);

    /// <summary>Draw rectangle outline with extended parameters</summary>
    [DllImport("raylib", CallingConvention = (CallingConvention) 2)]
    public static extern void DrawRectangleLinesEx(Rectangle rec, float lineThick, Color color);

    /// <summary>Draw rectangle with rounded edges</summary>
    [DllImport("raylib", CallingConvention = (CallingConvention) 2)]
    public static extern void DrawRectangleRounded(
      Rectangle rec,
      float roundness,
      int segments,
      Color color);

    /// <summary>Draw rectangle with rounded edges outline</summary>
    [DllImport("raylib", CallingConvention = (CallingConvention) 2)]
    public static extern void DrawRectangleRoundedLines(
      Rectangle rec,
      float roundness,
      int segments,
      float lineThick,
      Color color);

    /// <summary>Draw a color-filled triangle (vertex in counter-clockwise order!)</summary>
    [DllImport("raylib", CallingConvention = (CallingConvention) 2)]
    public static extern void DrawTriangle(Vector2 v1, Vector2 v2, Vector2 v3, Color color);

    /// <summary>Draw triangle outline (vertex in counter-clockwise order!)</summary>
    [DllImport("raylib", CallingConvention = (CallingConvention) 2)]
    public static extern void DrawTriangleLines(Vector2 v1, Vector2 v2, Vector2 v3, Color color);

    /// <summary>Draw a triangle fan defined by points (first vertex is the center)</summary>
    [DllImport("raylib", CallingConvention = (CallingConvention) 2)]
    public static extern unsafe void DrawTriangleFan(Vector2* points, int pointCount, Color color);

    /// <summary>Draw a triangle strip defined by points</summary>
    [DllImport("raylib", CallingConvention = (CallingConvention) 2)]
    public static extern unsafe void DrawTriangleStrip(
      Vector2* points,
      int pointCount,
      Color color);

    /// <summary>Draw a regular polygon (Vector version)</summary>
    [DllImport("raylib", CallingConvention = (CallingConvention) 2)]
    public static extern void DrawPoly(
      Vector2 center,
      int sides,
      float radius,
      float rotation,
      Color color);

    /// <summary>Draw a polygon outline of n sides</summary>
    [DllImport("raylib", CallingConvention = (CallingConvention) 2)]
    public static extern void DrawPolyLines(
      Vector2 center,
      int sides,
      float radius,
      float rotation,
      Color color);

    /// <summary>Draw a polygon outline of n sides with extended parameters</summary>
    [DllImport("raylib", CallingConvention = (CallingConvention) 2)]
    public static extern void DrawPolyLinesEx(
      Vector2 center,
      int sides,
      float radius,
      float rotation,
      float lineThick,
      Color color);

    /// <summary>Check collision between two rectangles</summary>
    [DllImport("raylib", CallingConvention = (CallingConvention) 2)]
    public static extern CBool CheckCollisionRecs(Rectangle rec1, Rectangle rec2);

    /// <summary>Check collision between two circles</summary>
    [DllImport("raylib", CallingConvention = (CallingConvention) 2)]
    public static extern CBool CheckCollisionCircles(
      Vector2 center1,
      float radius1,
      Vector2 center2,
      float radius2);

    /// <summary>Check collision between circle and rectangle</summary>
    [DllImport("raylib", CallingConvention = (CallingConvention) 2)]
    public static extern CBool CheckCollisionCircleRec(Vector2 center, float radius, Rectangle rec);

    /// <summary>Check if point is inside rectangle</summary>
    [DllImport("raylib", CallingConvention = (CallingConvention) 2)]
    public static extern CBool CheckCollisionPointRec(Vector2 point, Rectangle rec);

    /// <summary>Check if point is inside circle</summary>
    [DllImport("raylib", CallingConvention = (CallingConvention) 2)]
    public static extern CBool CheckCollisionPointCircle(
      Vector2 point,
      Vector2 center,
      float radius);

    /// <summary>Check if point is inside a triangle</summary>
    [DllImport("raylib", CallingConvention = (CallingConvention) 2)]
    public static extern CBool CheckCollisionPointTriangle(
      Vector2 point,
      Vector2 p1,
      Vector2 p2,
      Vector2 p3);

    /// <summary>Check if point is within a polygon described by array of vertices</summary>
    [DllImport("raylib", CallingConvention = (CallingConvention) 2)]
    public static extern unsafe CBool CheckCollisionPointPoly(
      Vector2 point,
      Vector2* points,
      int pointCount);

    /// <summary>
    /// Check the collision between two lines defined by two points each, returns collision point by reference
    /// </summary>
    [DllImport("raylib", CallingConvention = (CallingConvention) 2)]
    public static extern unsafe CBool CheckCollisionLines(
      Vector2 startPos1,
      Vector2 endPos1,
      Vector2 startPos2,
      Vector2 endPos2,
      Vector2* collisionPoint);

    /// <summary>
    /// Check if point belongs to line created between two points [p1] and [p2] with defined margin in pixels [threshold]
    /// </summary>
    [DllImport("raylib", CallingConvention = (CallingConvention) 2)]
    public static extern CBool CheckCollisionPointLine(
      Vector2 point,
      Vector2 p1,
      Vector2 p2,
      int threshold);

    /// <summary>Get collision rectangle for two rectangles collision</summary>
    [DllImport("raylib", CallingConvention = (CallingConvention) 2)]
    public static extern Rectangle GetCollisionRec(Rectangle rec1, Rectangle rec2);

    /// <summary>Load image from file into CPU memory (RAM)</summary>
    [DllImport("raylib", CallingConvention = (CallingConvention) 2)]
    public static extern unsafe Image LoadImage(sbyte* fileName);

    /// <summary>Load image from RAW file data</summary>
    [DllImport("raylib", CallingConvention = (CallingConvention) 2)]
    public static extern unsafe Image LoadImageRaw(
      sbyte* fileName,
      int width,
      int height,
      PixelFormat format,
      int headerSize);

    /// <summary>Load image sequence from file (frames appended to image.data)</summary>
    [DllImport("raylib", CallingConvention = (CallingConvention) 2)]
    public static extern unsafe Image LoadImageAnim(sbyte* fileName, int* frames);

    /// <summary>Load image from memory buffer, fileType refers to extension: i.e. "png"</summary>
    [DllImport("raylib", CallingConvention = (CallingConvention) 2)]
    public static extern unsafe Image LoadImageFromMemory(
      sbyte* fileType,
      byte* fileData,
      int dataSize);

    /// <summary>Load image from GPU texture data</summary>
    [DllImport("raylib", CallingConvention = (CallingConvention) 2)]
    public static extern Image LoadImageFromTexture(Texture2D texture);

    /// <summary>Load image from screen buffer and (screenshot)</summary>
    [DllImport("raylib", CallingConvention = (CallingConvention) 2)]
    public static extern Image LoadImageFromScreen();

    /// <summary>Check if an image is ready</summary>
    [DllImport("raylib", CallingConvention = (CallingConvention) 2)]
    public static extern CBool IsImageReady(Image image);

    /// <summary>Unload image from CPU memory (RAM)</summary>
    [DllImport("raylib", CallingConvention = (CallingConvention) 2)]
    public static extern void UnloadImage(Image image);

    /// <summary>Export image data to file</summary>
    [DllImport("raylib", CallingConvention = (CallingConvention) 2)]
    public static extern unsafe CBool ExportImage(Image image, sbyte* fileName);

    /// <summary>Export image as code file defining an array of bytes</summary>
    [DllImport("raylib", CallingConvention = (CallingConvention) 2)]
    public static extern unsafe CBool ExportImageAsCode(Image image, sbyte* fileName);

    /// <summary>Generate image: plain color</summary>
    [DllImport("raylib", CallingConvention = (CallingConvention) 2)]
    public static extern Image GenImageColor(int width, int height, Color color);

    /// <summary>Generate image: vertical gradient</summary>
    [DllImport("raylib", CallingConvention = (CallingConvention) 2)]
    public static extern Image GenImageGradientV(int width, int height, Color top, Color bottom);

    /// <summary>Generate image: horizontal gradient</summary>
    [DllImport("raylib", CallingConvention = (CallingConvention) 2)]
    public static extern Image GenImageGradientH(int width, int height, Color left, Color right);

    /// <summary>Generate image: radial gradient</summary>
    [DllImport("raylib", CallingConvention = (CallingConvention) 2)]
    public static extern Image GenImageGradientRadial(
      int width,
      int height,
      float density,
      Color inner,
      Color outer);

    /// <summary>Generate image: checked</summary>
    [DllImport("raylib", CallingConvention = (CallingConvention) 2)]
    public static extern Image GenImageChecked(
      int width,
      int height,
      int checksX,
      int checksY,
      Color col1,
      Color col2);

    /// <summary>Generate image: white noise</summary>
    [DllImport("raylib", CallingConvention = (CallingConvention) 2)]
    public static extern Image GenImageWhiteNoise(int width, int height, float factor);

    /// <summary>Generate image: perlin noise</summary>
    [DllImport("raylib", CallingConvention = (CallingConvention) 2)]
    public static extern Image GenImagePerlinNoise(
      int width,
      int height,
      int offsetX,
      int offsetY,
      float scale);

    /// <summary>Generate image: cellular algorithm, bigger tileSize means bigger cells</summary>
    [DllImport("raylib", CallingConvention = (CallingConvention) 2)]
    public static extern Image GenImageCellular(int width, int height, int tileSize);

    /// <summary>Generate image: grayscale image from text data</summary>
    [DllImport("raylib", CallingConvention = (CallingConvention) 2)]
    public static extern Image GenImageText(int width, int height, int tileSize);

    /// <summary>Create an image duplicate (useful for transformations)</summary>
    [DllImport("raylib", CallingConvention = (CallingConvention) 2)]
    public static extern Image ImageCopy(Image image);

    /// <summary>Create an image from another image piece</summary>
    [DllImport("raylib", CallingConvention = (CallingConvention) 2)]
    public static extern Image ImageFromImage(Image image, Rectangle rec);

    /// <summary>Create an image from text (default font)</summary>
    [DllImport("raylib", CallingConvention = (CallingConvention) 2)]
    public static extern unsafe Image ImageText(sbyte* text, int fontSize, Color color);

    /// <summary>Create an image from text (custom sprite font)</summary>
    [DllImport("raylib", CallingConvention = (CallingConvention) 2)]
    public static extern unsafe Image ImageTextEx(
      Font font,
      sbyte* text,
      float fontSize,
      float spacing,
      Color tint);

    /// <summary>Convert image data to desired format</summary>
    [DllImport("raylib", CallingConvention = (CallingConvention) 2)]
    public static extern unsafe void ImageFormat(Image* image, PixelFormat newFormat);

    /// <summary>Convert image to POT (power-of-two)</summary>
    [DllImport("raylib", CallingConvention = (CallingConvention) 2)]
    public static extern unsafe void ImageToPOT(Image* image, Color fill);

    /// <summary>Crop an image to a defined rectangle</summary>
    [DllImport("raylib", CallingConvention = (CallingConvention) 2)]
    public static extern unsafe void ImageCrop(Image* image, Rectangle crop);

    /// <summary>Crop image depending on alpha value</summary>
    [DllImport("raylib", CallingConvention = (CallingConvention) 2)]
    public static extern unsafe void ImageAlphaCrop(Image* image, float threshold);

    /// <summary>Clear alpha channel to desired color</summary>
    [DllImport("raylib", CallingConvention = (CallingConvention) 2)]
    public static extern unsafe void ImageAlphaClear(Image* image, Color color, float threshold);

    /// <summary>Apply alpha mask to image</summary>
    [DllImport("raylib", CallingConvention = (CallingConvention) 2)]
    public static extern unsafe void ImageAlphaMask(Image* image, Image alphaMask);

    /// <summary>Premultiply alpha channel</summary>
    [DllImport("raylib", CallingConvention = (CallingConvention) 2)]
    public static extern unsafe void ImageAlphaPremultiply(Image* image);

    /// <summary>Apply Gaussian blur using a box blur approximation</summary>
    [DllImport("raylib", CallingConvention = (CallingConvention) 2)]
    public static extern unsafe void ImageBlurGaussian(Image* image, int blurSize);

    /// <summary>Resize image (Bicubic scaling algorithm)</summary>
    [DllImport("raylib", CallingConvention = (CallingConvention) 2)]
    public static extern unsafe void ImageResize(Image* image, int newWidth, int newHeight);

    /// <summary>Resize image (Nearest-Neighbor scaling algorithm)</summary>
    [DllImport("raylib", CallingConvention = (CallingConvention) 2)]
    public static extern unsafe void ImageResizeNN(Image* image, int newWidth, int newHeight);

    /// <summary>Resize canvas and fill with color</summary>
    [DllImport("raylib", CallingConvention = (CallingConvention) 2)]
    public static extern unsafe void ImageResizeCanvas(
      Image* image,
      int newWidth,
      int newHeight,
      int offsetX,
      int offsetY,
      Color color);

    /// <summary>Generate all mipmap levels for a provided image</summary>
    [DllImport("raylib", CallingConvention = (CallingConvention) 2)]
    public static extern unsafe void ImageMipmaps(Image* image);

    /// <summary>Dither image data to 16bpp or lower (Floyd-Steinberg dithering)</summary>
    [DllImport("raylib", CallingConvention = (CallingConvention) 2)]
    public static extern unsafe void ImageDither(
      Image* image,
      int rBpp,
      int gBpp,
      int bBpp,
      int aBpp);

    /// <summary>Flip image vertically</summary>
    [DllImport("raylib", CallingConvention = (CallingConvention) 2)]
    public static extern unsafe void ImageFlipVertical(Image* image);

    /// <summary>Flip image horizontally</summary>
    [DllImport("raylib", CallingConvention = (CallingConvention) 2)]
    public static extern unsafe void ImageFlipHorizontal(Image* image);

    /// <summary>Rotate image clockwise 90deg</summary>
    [DllImport("raylib", CallingConvention = (CallingConvention) 2)]
    public static extern unsafe void ImageRotateCW(Image* image);

    /// <summary>Rotate image counter-clockwise 90deg</summary>
    [DllImport("raylib", CallingConvention = (CallingConvention) 2)]
    public static extern unsafe void ImageRotateCCW(Image* image);

    /// <summary>Modify image color: tint</summary>
    [DllImport("raylib", CallingConvention = (CallingConvention) 2)]
    public static extern unsafe void ImageColorTint(Image* image, Color color);

    /// <summary>Modify image color: invert</summary>
    [DllImport("raylib", CallingConvention = (CallingConvention) 2)]
    public static extern unsafe void ImageColorInvert(Image* image);

    /// <summary>Modify image color: grayscale</summary>
    [DllImport("raylib", CallingConvention = (CallingConvention) 2)]
    public static extern unsafe void ImageColorGrayscale(Image* image);

    /// <summary>Modify image color: contrast (-100 to 100)</summary>
    [DllImport("raylib", CallingConvention = (CallingConvention) 2)]
    public static extern unsafe void ImageColorContrast(Image* image, float contrast);

    /// <summary>Modify image color: brightness (-255 to 255)</summary>
    [DllImport("raylib", CallingConvention = (CallingConvention) 2)]
    public static extern unsafe void ImageColorBrightness(Image* image, int brightness);

    /// <summary>Modify image color: replace color</summary>
    [DllImport("raylib", CallingConvention = (CallingConvention) 2)]
    public static extern unsafe void ImageColorReplace(Image* image, Color color, Color replace);

    /// <summary>Load color data from image as a Color array (RGBA - 32bit)</summary>
    [DllImport("raylib", CallingConvention = (CallingConvention) 2)]
    public static extern unsafe Color* LoadImageColors(Image image);

    /// <summary>Load colors palette from image as a Color array (RGBA - 32bit)</summary>
    [DllImport("raylib", CallingConvention = (CallingConvention) 2)]
    public static extern unsafe Color* LoadImagePalette(
      Image image,
      int maxPaletteSize,
      int* colorCount);

    /// <summary>Unload color data loaded with LoadImageColors()</summary>
    [DllImport("raylib", CallingConvention = (CallingConvention) 2)]
    public static extern unsafe void UnloadImageColors(Color* colors);

    /// <summary>Unload colors palette loaded with LoadImagePalette()</summary>
    [DllImport("raylib", CallingConvention = (CallingConvention) 2)]
    public static extern unsafe void UnloadImagePalette(Color* colors);

    /// <summary>Get image alpha border rectangle</summary>
    [DllImport("raylib", CallingConvention = (CallingConvention) 2)]
    public static extern Rectangle GetImageAlphaBorder(Image image, float threshold);

    /// <summary>Get image pixel color at (x, y) position</summary>
    [DllImport("raylib", CallingConvention = (CallingConvention) 2)]
    public static extern Color GetImageColor(Image image, int x, int y);

    /// <summary>Clear image background with given color</summary>
    [DllImport("raylib", CallingConvention = (CallingConvention) 2)]
    public static extern unsafe void ImageClearBackground(Image* dst, Color color);

    /// <summary>Draw pixel within an image</summary>
    [DllImport("raylib", CallingConvention = (CallingConvention) 2)]
    public static extern unsafe void ImageDrawPixel(Image* dst, int posX, int posY, Color color);

    /// <summary>Draw pixel within an image (Vector version)</summary>
    [DllImport("raylib", CallingConvention = (CallingConvention) 2)]
    public static extern unsafe void ImageDrawPixelV(Image* dst, Vector2 position, Color color);

    /// <summary>Draw line within an image</summary>
    [DllImport("raylib", CallingConvention = (CallingConvention) 2)]
    public static extern unsafe void ImageDrawLine(
      Image* dst,
      int startPosX,
      int startPosY,
      int endPosX,
      int endPosY,
      Color color);

    /// <summary>Draw line within an image (Vector version)</summary>
    [DllImport("raylib", CallingConvention = (CallingConvention) 2)]
    public static extern unsafe void ImageDrawLineV(
      Image* dst,
      Vector2 start,
      Vector2 end,
      Color color);

    /// <summary>Draw circle within an image</summary>
    [DllImport("raylib", CallingConvention = (CallingConvention) 2)]
    public static extern unsafe void ImageDrawCircle(
      Image* dst,
      int centerX,
      int centerY,
      int radius,
      Color color);

    /// <summary>Draw circle within an image (Vector version)</summary>
    [DllImport("raylib", CallingConvention = (CallingConvention) 2)]
    public static extern unsafe void ImageDrawCircleV(
      Image* dst,
      Vector2 center,
      int radius,
      Color color);

    /// <summary>Draw circle outline within an image</summary>
    [DllImport("raylib", CallingConvention = (CallingConvention) 2)]
    public static extern unsafe void ImageDrawCircleLines(
      Image* dst,
      int centerX,
      int centerY,
      int radius,
      Color color);

    /// <summary>Draw circle outline within an image (Vector version)</summary>
    [DllImport("raylib", CallingConvention = (CallingConvention) 2)]
    public static extern unsafe void ImageDrawCircleLinesV(
      Image* dst,
      Vector2 center,
      int radius,
      Color color);

    /// <summary>Draw rectangle within an image</summary>
    [DllImport("raylib", CallingConvention = (CallingConvention) 2)]
    public static extern unsafe void ImageDrawRectangle(
      Image* dst,
      int posX,
      int posY,
      int width,
      int height,
      Color color);

    /// <summary>Draw rectangle within an image (Vector version)</summary>
    [DllImport("raylib", CallingConvention = (CallingConvention) 2)]
    public static extern unsafe void ImageDrawRectangleV(
      Image* dst,
      Vector2 position,
      Vector2 size,
      Color color);

    /// <summary>Draw rectangle within an image</summary>
    [DllImport("raylib", CallingConvention = (CallingConvention) 2)]
    public static extern unsafe void ImageDrawRectangleRec(Image* dst, Rectangle rec, Color color);

    /// <summary>Draw rectangle lines within an image</summary>
    [DllImport("raylib", CallingConvention = (CallingConvention) 2)]
    public static extern unsafe void ImageDrawRectangleLines(
      Image* dst,
      Rectangle rec,
      int thick,
      Color color);

    /// <summary>Draw a source image within a destination image (tint applied to source)</summary>
    [DllImport("raylib", CallingConvention = (CallingConvention) 2)]
    public static extern unsafe void ImageDraw(
      Image* dst,
      Image src,
      Rectangle srcRec,
      Rectangle dstRec,
      Color tint);

    /// <summary>Draw text (using default font) within an image (destination)</summary>
    [DllImport("raylib", CallingConvention = (CallingConvention) 2)]
    public static extern unsafe void ImageDrawText(
      Image* dst,
      sbyte* text,
      int x,
      int y,
      int fontSize,
      Color color);

    /// <summary>Draw text (custom sprite font) within an image (destination)</summary>
    [DllImport("raylib", CallingConvention = (CallingConvention) 2)]
    public static extern unsafe void ImageDrawTextEx(
      Image* dst,
      Font font,
      sbyte* text,
      Vector2 position,
      float fontSize,
      float spacing,
      Color tint);

    /// <summary>Load texture from file into GPU memory (VRAM)</summary>
    [DllImport("raylib", CallingConvention = (CallingConvention) 2)]
    public static extern unsafe Texture2D LoadTexture(sbyte* fileName);

    /// <summary>Load texture from image data</summary>
    [DllImport("raylib", CallingConvention = (CallingConvention) 2)]
    public static extern Texture2D LoadTextureFromImage(Image image);

    /// <summary>Load cubemap from image, multiple image cubemap layouts supported</summary>
    [DllImport("raylib", CallingConvention = (CallingConvention) 2)]
    public static extern Texture2D LoadTextureCubemap(Image image, CubemapLayout layout);

    /// <summary>Load texture for rendering (framebuffer)</summary>
    [DllImport("raylib", CallingConvention = (CallingConvention) 2)]
    public static extern RenderTexture2D LoadRenderTexture(int width, int height);

    /// <summary>Check if a texture is ready</summary>
    [DllImport("raylib", CallingConvention = (CallingConvention) 2)]
    public static extern CBool IsTextureReady(Texture2D texture);

    /// <summary>Unload texture from GPU memory (VRAM)</summary>
    [DllImport("raylib", CallingConvention = (CallingConvention) 2)]
    public static extern void UnloadTexture(Texture2D texture);

    /// <summary>Check if a render texture is ready</summary>
    [DllImport("raylib", CallingConvention = (CallingConvention) 2)]
    public static extern CBool IsRenderTextureReady(RenderTexture2D target);

    /// <summary>Unload render texture from GPU memory (VRAM)</summary>
    [DllImport("raylib", CallingConvention = (CallingConvention) 2)]
    public static extern void UnloadRenderTexture(RenderTexture2D target);

    /// <summary>Update GPU texture with new data</summary>
    [DllImport("raylib", CallingConvention = (CallingConvention) 2)]
    public static extern unsafe void UpdateTexture(Texture2D texture, void* pixels);

    /// <summary>Update GPU texture rectangle with new data</summary>
    [DllImport("raylib", CallingConvention = (CallingConvention) 2)]
    public static extern unsafe void UpdateTextureRec(
      Texture2D texture,
      Rectangle rec,
      void* pixels);

    /// <summary>Generate GPU mipmaps for a texture</summary>
    [DllImport("raylib", CallingConvention = (CallingConvention) 2)]
    public static extern unsafe void GenTextureMipmaps(Texture2D* texture);

    /// <summary>Set texture scaling filter mode</summary>
    [DllImport("raylib", CallingConvention = (CallingConvention) 2)]
    public static extern void SetTextureFilter(Texture2D texture, TextureFilter filter);

    /// <summary>Set texture wrapping mode</summary>
    [DllImport("raylib", CallingConvention = (CallingConvention) 2)]
    public static extern void SetTextureWrap(Texture2D texture, TextureWrap wrap);

    /// <summary>Draw a Texture2D</summary>
    [DllImport("raylib", CallingConvention = (CallingConvention) 2)]
    public static extern void DrawTexture(Texture2D texture, int posX, int posY, Color tint);

    /// <summary>Draw a Texture2D with position defined as Vector2</summary>
    [DllImport("raylib", CallingConvention = (CallingConvention) 2)]
    public static extern void DrawTextureV(Texture2D texture, Vector2 position, Color tint);

    /// <summary>Draw a Texture2D with extended parameters</summary>
    [DllImport("raylib", CallingConvention = (CallingConvention) 2)]
    public static extern void DrawTextureEx(
      Texture2D texture,
      Vector2 position,
      float rotation,
      float scale,
      Color tint);

    /// <summary>Draw a part of a texture defined by a rectangle</summary>
    [DllImport("raylib", CallingConvention = (CallingConvention) 2)]
    public static extern void DrawTextureRec(
      Texture2D texture,
      Rectangle source,
      Vector2 position,
      Color tint);

    /// <summary>Draw a part of a texture defined by a rectangle with 'pro' parameters</summary>
    [DllImport("raylib", CallingConvention = (CallingConvention) 2)]
    public static extern void DrawTexturePro(
      Texture2D texture,
      Rectangle source,
      Rectangle dest,
      Vector2 origin,
      float rotation,
      Color tint);

    /// <summary>Draws a texture (or part of it) that stretches or shrinks nicely</summary>
    [DllImport("raylib", CallingConvention = (CallingConvention) 2)]
    public static extern void DrawTextureNPatch(
      Texture2D texture,
      NPatchInfo nPatchInfo,
      Rectangle dest,
      Vector2 origin,
      float rotation,
      Color tint);

    /// <summary>Get hexadecimal value for a Color</summary>
    [DllImport("raylib", CallingConvention = (CallingConvention) 2)]
    public static extern int ColorToInt(Color color);

    /// <summary>Get color normalized as float [0..1]</summary>
    [DllImport("raylib", CallingConvention = (CallingConvention) 2)]
    public static extern Vector4 ColorNormalize(Color color);

    /// <summary>Get color from normalized values [0..1]</summary>
    [DllImport("raylib", CallingConvention = (CallingConvention) 2)]
    public static extern Color ColorFromNormalized(Vector4 normalized);

    /// <summary>Get HSV values for a Color, hue [0..360], saturation/value [0..1]</summary>
    [DllImport("raylib", CallingConvention = (CallingConvention) 2)]
    public static extern Vector3 ColorToHSV(Color color);

    /// <summary>Get a Color from HSV values, hue [0..360], saturation/value [0..1]</summary>
    [DllImport("raylib", CallingConvention = (CallingConvention) 2)]
    public static extern Color ColorFromHSV(float hue, float saturation, float value);

    /// <summary>Get color multiplied with another color</summary>
    [DllImport("raylib", CallingConvention = (CallingConvention) 2)]
    public static extern Color ColorTint(Color color, Color tint);

    /// <summary>Get color with brightness correction, brightness factor goes from -1.0f to 1.0f</summary>
    [DllImport("raylib", CallingConvention = (CallingConvention) 2)]
    public static extern Color ColorBrightness(Color color, float factor);

    /// <summary>Get color with contrast correction, contrast values between -1.0f and 1.0f</summary>
    [DllImport("raylib", CallingConvention = (CallingConvention) 2)]
    public static extern Color ColorContrast(Color color, float contrast);

    /// <summary>Get color with alpha applied, alpha goes from 0.0f to 1.0f</summary>
    [DllImport("raylib", CallingConvention = (CallingConvention) 2)]
    public static extern Color ColorAlpha(Color color, float alpha);

    /// <summary>Get src alpha-blended into dst color with tint</summary>
    [DllImport("raylib", CallingConvention = (CallingConvention) 2)]
    public static extern Color ColorAlphaBlend(Color dst, Color src, Color tint);

    /// <summary>Get Color structure from hexadecimal value</summary>
    [DllImport("raylib", CallingConvention = (CallingConvention) 2)]
    public static extern Color GetColor(uint hexValue);

    /// <summary>Get Color from a source pixel pointer of certain format</summary>
    [DllImport("raylib", CallingConvention = (CallingConvention) 2)]
    public static extern unsafe Color GetPixelColor(void* srcPtr, PixelFormat format);

    /// <summary>Set color formatted into destination pixel pointer</summary>
    [DllImport("raylib", CallingConvention = (CallingConvention) 2)]
    public static extern unsafe void SetPixelColor(void* dstPtr, Color color, PixelFormat format);

    /// <summary>Get pixel data size in bytes for certain format</summary>
    [DllImport("raylib", CallingConvention = (CallingConvention) 2)]
    public static extern int GetPixelDataSize(int width, int height, PixelFormat format);

    /// <summary>Get the default Font</summary>
    [DllImport("raylib", CallingConvention = (CallingConvention) 2)]
    public static extern Font GetFontDefault();

    /// <summary>Load font from file into GPU memory (VRAM)</summary>
    [DllImport("raylib", CallingConvention = (CallingConvention) 2)]
    public static extern unsafe Font LoadFont(sbyte* fileName);

    /// <summary>
    /// Load font from file with extended parameters, use NULL for fontChars and 0 for glyphCount to load
    /// the default character set
    /// </summary>
    [DllImport("raylib", CallingConvention = (CallingConvention) 2)]
    public static extern unsafe Font LoadFontEx(
      sbyte* fileName,
      int fontSize,
      int* fontChars,
      int glyphCount);

    /// <summary>Load font from Image (XNA style)</summary>
    [DllImport("raylib", CallingConvention = (CallingConvention) 2)]
    public static extern Font LoadFontFromImage(Image image, Color key, int firstChar);

    /// <summary>Load font from memory buffer, fileType refers to extension: i.e. "ttf"</summary>
    [DllImport("raylib", CallingConvention = (CallingConvention) 2)]
    public static extern unsafe Font LoadFontFromMemory(
      sbyte* fileType,
      byte* fileData,
      int dataSize,
      int fontSize,
      int* fontChars,
      int glyphCount);

    /// <summary>Check if a font is ready</summary>
    [DllImport("raylib", CallingConvention = (CallingConvention) 2)]
    public static extern CBool IsFontReady(Font font);

    /// <summary>Load font data for further use</summary>
    [DllImport("raylib", CallingConvention = (CallingConvention) 2)]
    public static extern unsafe GlyphInfo* LoadFontData(
      byte* fileData,
      int dataSize,
      int fontSize,
      int* fontChars,
      int glyphCount,
      FontType type);

    /// <summary>Generate image font atlas using chars info</summary>
    [DllImport("raylib", CallingConvention = (CallingConvention) 2)]
    public static extern unsafe Image GenImageFontAtlas(
      GlyphInfo* chars,
      Rectangle** recs,
      int glyphCount,
      int fontSize,
      int padding,
      int packMethod);

    /// <summary>Unload font chars info data (RAM)</summary>
    [DllImport("raylib", CallingConvention = (CallingConvention) 2)]
    public static extern unsafe void UnloadFontData(GlyphInfo* chars, int glyphCount);

    /// <summary>Unload Font from GPU memory (VRAM)</summary>
    [DllImport("raylib", CallingConvention = (CallingConvention) 2)]
    public static extern void UnloadFont(Font font);

    /// <summary>Export font as code file, returns true on success</summary>
    [DllImport("raylib", CallingConvention = (CallingConvention) 2)]
    public static extern unsafe CBool ExportFontAsCode(Font font, sbyte* fileName);

    /// <summary>Shows current FPS</summary>
    [DllImport("raylib", CallingConvention = (CallingConvention) 2)]
    public static extern void DrawFPS(int posX, int posY);

    /// <summary>Draw text (using default font)</summary>
    [DllImport("raylib", CallingConvention = (CallingConvention) 2)]
    public static extern unsafe void DrawText(
      sbyte* text,
      int posX,
      int posY,
      int fontSize,
      Color color);

    /// <summary>Draw text using font and additional parameters</summary>
    [DllImport("raylib", CallingConvention = (CallingConvention) 2)]
    public static extern unsafe void DrawTextEx(
      Font font,
      sbyte* text,
      Vector2 position,
      float fontSize,
      float spacing,
      Color tint);

    /// <summary>Draw text using Font and pro parameters (rotation)</summary>
    [DllImport("raylib", CallingConvention = (CallingConvention) 2)]
    public static extern unsafe void DrawTextPro(
      Font font,
      sbyte* text,
      Vector2 position,
      Vector2 origin,
      float rotation,
      float fontSize,
      float spacing,
      Color tint);

    /// <summary>Draw one character (codepoint)</summary>
    [DllImport("raylib", CallingConvention = (CallingConvention) 2)]
    public static extern void DrawTextCodepoint(
      Font font,
      int codepoint,
      Vector2 position,
      float fontSize,
      Color tint);

    /// <summary>Draw multiple characters (codepoint)</summary>
    [DllImport("raylib", CallingConvention = (CallingConvention) 2)]
    public static extern unsafe void DrawTextCodepoints(
      Font font,
      int* codepoints,
      int count,
      Vector2 position,
      float fontSize,
      float spacing,
      Color tint);

    /// <summary>Measure string width for default font</summary>
    [DllImport("raylib", CallingConvention = (CallingConvention) 2)]
    public static extern unsafe int MeasureText(sbyte* text, int fontSize);

    /// <summary>Measure string size for Font</summary>
    [DllImport("raylib", CallingConvention = (CallingConvention) 2)]
    public static extern unsafe Vector2 MeasureTextEx(
      Font font,
      sbyte* text,
      float fontSize,
      float spacing);

    /// <summary>
    /// Get glyph index position in font for a codepoint (unicode character), fallback to '?' if not found
    /// </summary>
    [DllImport("raylib", CallingConvention = (CallingConvention) 2)]
    public static extern int GetGlyphIndex(Font font, int character);

    /// <summary>
    /// Get glyph font info data for a codepoint (unicode character), fallback to '?' if not found
    /// </summary>
    [DllImport("raylib", CallingConvention = (CallingConvention) 2)]
    public static extern GlyphInfo GetGlyphInfo(Font font, int codepoint);

    /// <summary>
    /// Get glyph rectangle in font atlas for a codepoint (unicode character), fallback to '?' if not found
    /// </summary>
    [DllImport("raylib", CallingConvention = (CallingConvention) 2)]
    public static extern Rectangle GetGlyphAtlasRec(Font font, int codepoint);

    /// <summary>Load UTF-8 text encoded from codepoints array</summary>
    [DllImport("raylib", CallingConvention = (CallingConvention) 2)]
    public static extern unsafe sbyte* LoadUTF8(int* codepoints, int length);

    /// <summary>Unload UTF-8 text encoded from codepoints array</summary>
    [DllImport("raylib", CallingConvention = (CallingConvention) 2)]
    public static extern unsafe void UnloadUTF8(int* text);

    /// <summary>Load all codepoints from a UTF-8 text string, codepoints count returned by parameter</summary>
    [DllImport("raylib", CallingConvention = (CallingConvention) 2)]
    public static extern unsafe int* LoadCodepoints(sbyte* text, int* count);

    /// <summary>Unload codepoints data from memory</summary>
    [DllImport("raylib", CallingConvention = (CallingConvention) 2)]
    public static extern unsafe void UnloadCodepoints(int* codepoints);

    /// <summary>Get total number of codepoints in a UTF8 encoded string</summary>
    [DllImport("raylib", CallingConvention = (CallingConvention) 2)]
    public static extern unsafe int GetCodepointCount(sbyte* text);

    /// <summary>Get next codepoint in a UTF-8 encoded string, 0x3f('?') is returned on failure</summary>
    [DllImport("raylib", CallingConvention = (CallingConvention) 2)]
    public static extern unsafe int GetCodepoint(sbyte* text, int* codepointSize);

    /// <summary>Get next codepoint in a UTF-8 encoded string; 0x3f('?') is returned on failure</summary>
    [DllImport("raylib", CallingConvention = (CallingConvention) 2)]
    public static extern unsafe int GetCodepointNext(sbyte* text, int* codepointSize);

    /// <summary>Get previous codepoint in a UTF-8 encoded string, 0x3f('?') is returned on failure</summary>
    [DllImport("raylib", CallingConvention = (CallingConvention) 2)]
    public static extern unsafe int GetCodepointPrevious(sbyte* text, int* codepointSize);

    /// <summary>Encode one codepoint into UTF-8 byte array (array length returned as parameter)</summary>
    [DllImport("raylib", CallingConvention = (CallingConvention) 2)]
    public static extern unsafe sbyte* CodepointToUTF8(int codepoint, int* utf8Size);

    [DllImport("raylib", CallingConvention = (CallingConvention) 2)]
    public static extern unsafe int TextCopy(char* dst, sbyte* src);

    /// <summary>Check if two text string are equal</summary>
    [DllImport("raylib", CallingConvention = (CallingConvention) 2)]
    public static extern unsafe CBool TextIsEqual(sbyte* text1, sbyte* text2);

    /// <summary>Get text length, checks for '\0' ending</summary>
    [DllImport("raylib", CallingConvention = (CallingConvention) 2)]
    public static extern unsafe uint TextLength(sbyte* text);

    /// <summary>Text formatting with variables (sprintf style)</summary>
    [DllImport("raylib", CallingConvention = (CallingConvention) 2)]
    public static extern unsafe sbyte* TextFormat(sbyte* text);

    /// <summary>Get a piece of a text string</summary>
    [DllImport("raylib", CallingConvention = (CallingConvention) 2)]
    public static extern unsafe sbyte* TextSubtext(sbyte* text, int position, int length);

    /// <summary>Replace text string (WARNING: memory must be freed!)</summary>
    [DllImport("raylib", CallingConvention = (CallingConvention) 2)]
    public static extern unsafe char* TextReplace(char* text, sbyte* replace, sbyte* by);

    /// <summary>Insert text in a position (WARNING: memory must be freed!)</summary>
    [DllImport("raylib", CallingConvention = (CallingConvention) 2)]
    public static extern unsafe char* TextInsert(sbyte* text, sbyte* insert, int position);

    /// <summary>Join text strings with delimiter</summary>
    [DllImport("raylib", CallingConvention = (CallingConvention) 2)]
    public static extern unsafe sbyte* TextJoin(sbyte** textList, int count, sbyte* delimiter);

    /// <summary>Split text into multiple strings</summary>
    [DllImport("raylib", CallingConvention = (CallingConvention) 2)]
    public static extern unsafe sbyte** TextSplit(sbyte* text, char delimiter, int* count);

    /// <summary>Append text at specific position and move cursor!</summary>
    [DllImport("raylib", CallingConvention = (CallingConvention) 2)]
    public static extern unsafe void TextAppend(sbyte* text, sbyte* append, int* position);

    /// <summary>Find first text occurrence within a string</summary>
    [DllImport("raylib", CallingConvention = (CallingConvention) 2)]
    public static extern unsafe int TextFindIndex(sbyte* text, sbyte* find);

    /// <summary>Get upper case version of provided string</summary>
    [DllImport("raylib", CallingConvention = (CallingConvention) 2)]
    public static extern unsafe sbyte* TextToUpper(sbyte* text);

    /// <summary>Get lower case version of provided string</summary>
    [DllImport("raylib", CallingConvention = (CallingConvention) 2)]
    public static extern unsafe sbyte* TextToLower(sbyte* text);

    /// <summary>Get Pascal case notation version of provided string</summary>
    [DllImport("raylib", CallingConvention = (CallingConvention) 2)]
    public static extern unsafe sbyte* TextToPascal(sbyte* text);

    /// <summary>Get integer value from text (negative values not supported)</summary>
    [DllImport("raylib", CallingConvention = (CallingConvention) 2)]
    public static extern unsafe int TextToInteger(sbyte* text);

    /// <summary>Draw a line in 3D world space</summary>
    [DllImport("raylib", CallingConvention = (CallingConvention) 2)]
    public static extern void DrawLine3D(Vector3 startPos, Vector3 endPos, Color color);

    /// <summary>Draw a point in 3D space, actually a small line</summary>
    [DllImport("raylib", CallingConvention = (CallingConvention) 2)]
    public static extern void DrawPoint3D(Vector3 position, Color color);

    /// <summary>Draw a circle in 3D world space</summary>
    [DllImport("raylib", CallingConvention = (CallingConvention) 2)]
    public static extern void DrawCircle3D(
      Vector3 center,
      float radius,
      Vector3 rotationAxis,
      float rotationAngle,
      Color color);

    /// <summary>Draw a color-filled triangle (vertex in counter-clockwise order!)</summary>
    [DllImport("raylib", CallingConvention = (CallingConvention) 2)]
    public static extern void DrawTriangle3D(Vector3 v1, Vector3 v2, Vector3 v3, Color color);

    /// <summary>Draw a triangle strip defined by points</summary>
    [DllImport("raylib", CallingConvention = (CallingConvention) 2)]
    public static extern unsafe void DrawTriangleStrip3D(
      Vector3* points,
      int pointCount,
      Color color);

    /// <summary>Draw cube</summary>
    [DllImport("raylib", CallingConvention = (CallingConvention) 2)]
    public static extern void DrawCube(
      Vector3 position,
      float width,
      float height,
      float length,
      Color color);

    /// <summary>Draw cube (Vector version)</summary>
    [DllImport("raylib", CallingConvention = (CallingConvention) 2)]
    public static extern void DrawCubeV(Vector3 position, Vector3 size, Color color);

    /// <summary>Draw cube wires</summary>
    [DllImport("raylib", CallingConvention = (CallingConvention) 2)]
    public static extern void DrawCubeWires(
      Vector3 position,
      float width,
      float height,
      float length,
      Color color);

    /// <summary>Draw cube wires (Vector version)</summary>
    [DllImport("raylib", CallingConvention = (CallingConvention) 2)]
    public static extern void DrawCubeWiresV(Vector3 position, Vector3 size, Color color);

    /// <summary>Draw sphere</summary>
    [DllImport("raylib", CallingConvention = (CallingConvention) 2)]
    public static extern void DrawSphere(Vector3 centerPos, float radius, Color color);

    /// <summary>Draw sphere with extended parameters</summary>
    [DllImport("raylib", CallingConvention = (CallingConvention) 2)]
    public static extern void DrawSphereEx(
      Vector3 centerPos,
      float radius,
      int rings,
      int slices,
      Color color);

    /// <summary>Draw sphere wires</summary>
    [DllImport("raylib", CallingConvention = (CallingConvention) 2)]
    public static extern void DrawSphereWires(
      Vector3 centerPos,
      float radius,
      int rings,
      int slices,
      Color color);

    /// <summary>Draw a cylinder/cone</summary>
    [DllImport("raylib", CallingConvention = (CallingConvention) 2)]
    public static extern void DrawCylinder(
      Vector3 position,
      float radiusTop,
      float radiusBottom,
      float height,
      int slices,
      Color color);

    /// <summary>Draw a cylinder with base at startPos and top at endPos</summary>
    [DllImport("raylib", CallingConvention = (CallingConvention) 2)]
    public static extern void DrawCylinderEx(
      Vector3 startPos,
      Vector3 endPos,
      float startRadius,
      float endRadius,
      int sides,
      Color color);

    /// <summary>Draw a cylinder/cone wires</summary>
    [DllImport("raylib", CallingConvention = (CallingConvention) 2)]
    public static extern void DrawCylinderWires(
      Vector3 position,
      float radiusTop,
      float radiusBottom,
      float height,
      int slices,
      Color color);

    /// <summary>Draw a cylinder wires with base at startPos and top at endPos</summary>
    [DllImport("raylib", CallingConvention = (CallingConvention) 2)]
    public static extern void DrawCylinderWiresEx(
      Vector3 startPos,
      Vector3 endPos,
      float startRadius,
      float endRadius,
      int sides,
      Color color);

    /// <summary>Draw a capsule with the center of its sphere caps at startPos and endPos</summary>
    [DllImport("raylib", CallingConvention = (CallingConvention) 2)]
    public static extern void DrawCapsule(
      Vector3 startPos,
      Vector3 endPos,
      float radius,
      int slices,
      int rings,
      Color color);

    /// <summary>Draw capsule wireframe with the center of its sphere caps at startPos and endPos</summary>
    [DllImport("raylib", CallingConvention = (CallingConvention) 2)]
    public static extern void DrawCapsuleWires(
      Vector3 startPos,
      Vector3 endPos,
      float radius,
      int slices,
      int rings,
      Color color);

    /// <summary>Draw a plane XZ</summary>
    [DllImport("raylib", CallingConvention = (CallingConvention) 2)]
    public static extern void DrawPlane(Vector3 centerPos, Vector2 size, Color color);

    /// <summary>Draw a ray line</summary>
    [DllImport("raylib", CallingConvention = (CallingConvention) 2)]
    public static extern void DrawRay(Ray ray, Color color);

    /// <summary>Draw a grid (centered at (0, 0, 0))</summary>
    [DllImport("raylib", CallingConvention = (CallingConvention) 2)]
    public static extern void DrawGrid(int slices, float spacing);

    /// <summary>Load model from files (meshes and materials)</summary>
    [DllImport("raylib", CallingConvention = (CallingConvention) 2)]
    public static extern unsafe Model LoadModel(sbyte* fileName);

    /// <summary>Load model from generated mesh (default material)</summary>
    [DllImport("raylib", CallingConvention = (CallingConvention) 2)]
    public static extern Model LoadModelFromMesh(Mesh mesh);

    /// <summary>Check if a model is ready</summary>
    [DllImport("raylib", CallingConvention = (CallingConvention) 2)]
    public static extern CBool IsModelReady(Model model);

    /// <summary>Unload model from memory (RAM and/or VRAM)</summary>
    [DllImport("raylib", CallingConvention = (CallingConvention) 2)]
    public static extern void UnloadModel(Model model);

    /// <summary>Compute model bounding box limits (considers all meshes)</summary>
    [DllImport("raylib", CallingConvention = (CallingConvention) 2)]
    public static extern BoundingBox GetModelBoundingBox(Model model);

    /// <summary>Draw a model (with texture if set)</summary>
    [DllImport("raylib", CallingConvention = (CallingConvention) 2)]
    public static extern void DrawModel(Model model, Vector3 position, float scale, Color tint);

    /// <summary>Draw a model with extended parameters</summary>
    [DllImport("raylib", CallingConvention = (CallingConvention) 2)]
    public static extern void DrawModelEx(
      Model model,
      Vector3 position,
      Vector3 rotationAxis,
      float rotationAngle,
      Vector3 scale,
      Color tint);

    /// <summary>Draw a model wires (with texture if set)</summary>
    [DllImport("raylib", CallingConvention = (CallingConvention) 2)]
    public static extern void DrawModelWires(
      Model model,
      Vector3 position,
      float scale,
      Color tint);

    /// <summary>Draw a model wires (with texture if set) with extended parameters</summary>
    [DllImport("raylib", CallingConvention = (CallingConvention) 2)]
    public static extern void DrawModelWiresEx(
      Model model,
      Vector3 position,
      Vector3 rotationAxis,
      float rotationAngle,
      Vector3 scale,
      Color tint);

    /// <summary>Draw bounding box (wires)</summary>
    [DllImport("raylib", CallingConvention = (CallingConvention) 2)]
    public static extern void DrawBoundingBox(BoundingBox box, Color color);

    /// <summary>Draw a billboard texture</summary>
    [DllImport("raylib", CallingConvention = (CallingConvention) 2)]
    public static extern void DrawBillboard(
      Camera3D camera,
      Texture2D texture,
      Vector3 center,
      float size,
      Color tint);

    /// <summary>Draw a billboard texture defined by source</summary>
    [DllImport("raylib", CallingConvention = (CallingConvention) 2)]
    public static extern void DrawBillboardRec(
      Camera3D camera,
      Texture2D texture,
      Rectangle source,
      Vector3 position,
      Vector2 size,
      Color tint);

    /// <summary>Draw a billboard texture defined by source and rotation</summary>
    [DllImport("raylib", CallingConvention = (CallingConvention) 2)]
    public static extern void DrawBillboardPro(
      Camera3D camera,
      Texture2D texture,
      Rectangle source,
      Vector3 position,
      Vector3 up,
      Vector2 size,
      Vector2 origin,
      float rotation,
      Color tint);

    /// <summary>Upload vertex data into GPU and provided VAO/VBO ids</summary>
    [DllImport("raylib", CallingConvention = (CallingConvention) 2)]
    public static extern unsafe void UploadMesh(Mesh* mesh, CBool dynamic);

    /// <summary>Update mesh vertex data in GPU for a specific buffer index</summary>
    [DllImport("raylib", CallingConvention = (CallingConvention) 2)]
    public static extern unsafe void UpdateMeshBuffer(
      Mesh mesh,
      int index,
      void* data,
      int dataSize,
      int offset);

    /// <summary>Unload mesh from memory (RAM and/or VRAM)</summary>
    [DllImport("raylib", CallingConvention = (CallingConvention) 2)]
    public static extern unsafe void UnloadMesh(Mesh* mesh);

    /// <summary>Draw a 3d mesh with material and transform</summary>
    [DllImport("raylib", CallingConvention = (CallingConvention) 2)]
    public static extern void DrawMesh(Mesh mesh, Material material, Matrix4x4 transform);

    /// <summary>Draw multiple mesh instances with material and different transforms</summary>
    [DllImport("raylib", CallingConvention = (CallingConvention) 2)]
    public static extern unsafe void DrawMeshInstanced(
      Mesh mesh,
      Material material,
      Matrix4x4* transforms,
      int instances);

    /// <summary>Export mesh data to file, returns true on success</summary>
    [DllImport("raylib", CallingConvention = (CallingConvention) 2)]
    public static extern unsafe CBool ExportMesh(Mesh mesh, sbyte* fileName);

    /// <summary>Compute mesh bounding box limits</summary>
    [DllImport("raylib", CallingConvention = (CallingConvention) 2)]
    public static extern BoundingBox GetMeshBoundingBox(Mesh mesh);

    /// <summary>Compute mesh tangents</summary>
    [DllImport("raylib", CallingConvention = (CallingConvention) 2)]
    public static extern unsafe void GenMeshTangents(Mesh* mesh);

    /// <summary>Generate polygonal mesh</summary>
    [DllImport("raylib", CallingConvention = (CallingConvention) 2)]
    public static extern Mesh GenMeshPoly(int sides, float radius);

    /// <summary>Generate plane mesh (with subdivisions)</summary>
    [DllImport("raylib", CallingConvention = (CallingConvention) 2)]
    public static extern Mesh GenMeshPlane(float width, float length, int resX, int resZ);

    /// <summary>Generate cuboid mesh</summary>
    [DllImport("raylib", CallingConvention = (CallingConvention) 2)]
    public static extern Mesh GenMeshCube(float width, float height, float length);

    /// <summary>Generate sphere mesh (standard sphere)</summary>
    [DllImport("raylib", CallingConvention = (CallingConvention) 2)]
    public static extern Mesh GenMeshSphere(float radius, int rings, int slices);

    /// <summary>Generate half-sphere mesh (no bottom cap)</summary>
    [DllImport("raylib", CallingConvention = (CallingConvention) 2)]
    public static extern Mesh GenMeshHemiSphere(float radius, int rings, int slices);

    /// <summary>Generate cylinder mesh</summary>
    [DllImport("raylib", CallingConvention = (CallingConvention) 2)]
    public static extern Mesh GenMeshCylinder(float radius, float height, int slices);

    /// <summary>Generate cone/pyramid mesh</summary>
    [DllImport("raylib", CallingConvention = (CallingConvention) 2)]
    public static extern Mesh GenMeshCone(float radius, float height, int slices);

    /// <summary>Generate torus mesh</summary>
    [DllImport("raylib", CallingConvention = (CallingConvention) 2)]
    public static extern Mesh GenMeshTorus(float radius, float size, int radSeg, int sides);

    /// <summary>Generate trefoil knot mesh</summary>
    [DllImport("raylib", CallingConvention = (CallingConvention) 2)]
    public static extern Mesh GenMeshKnot(float radius, float size, int radSeg, int sides);

    /// <summary>Generate heightmap mesh from image data</summary>
    [DllImport("raylib", CallingConvention = (CallingConvention) 2)]
    public static extern Mesh GenMeshHeightmap(Image heightmap, Vector3 size);

    /// <summary>Generate cubes-based map mesh from image data</summary>
    [DllImport("raylib", CallingConvention = (CallingConvention) 2)]
    public static extern Mesh GenMeshCubicmap(Image cubicmap, Vector3 cubeSize);

    /// <summary>Load materials from model file</summary>
    [DllImport("raylib", CallingConvention = (CallingConvention) 2)]
    public static extern unsafe Material* LoadMaterials(sbyte* fileName, int* materialCount);

    /// <summary>Load default material (Supports: DIFFUSE, SPECULAR, NORMAL maps)</summary>
    [DllImport("raylib", CallingConvention = (CallingConvention) 2)]
    public static extern Material LoadMaterialDefault();

    /// <summary>Check if a material is ready</summary>
    [DllImport("raylib", CallingConvention = (CallingConvention) 2)]
    public static extern CBool IsMaterialReady();

    /// <summary>Unload material from GPU memory (VRAM)</summary>
    [DllImport("raylib", CallingConvention = (CallingConvention) 2)]
    public static extern void UnloadMaterial(Material material);

    /// <summary>Set texture for a material map type (MAP_DIFFUSE, MAP_SPECULAR...)</summary>
    [DllImport("raylib", CallingConvention = (CallingConvention) 2)]
    public static extern unsafe void SetMaterialTexture(
      Material* material,
      MaterialMapIndex mapType,
      Texture2D texture);

    /// <summary>Set material for a mesh</summary>
    [DllImport("raylib", CallingConvention = (CallingConvention) 2)]
    public static extern unsafe void SetModelMeshMaterial(Model* model, int meshId, int materialId);

    /// <summary>Load model animations from file</summary>
    [DllImport("raylib", CallingConvention = (CallingConvention) 2)]
    public static extern unsafe ModelAnimation* LoadModelAnimations(
      sbyte* fileName,
      uint* animCount);

    /// <summary>Update model animation pose</summary>
    [DllImport("raylib", CallingConvention = (CallingConvention) 2)]
    public static extern void UpdateModelAnimation(Model model, ModelAnimation anim, int frame);

    /// <summary>Unload animation data</summary>
    [DllImport("raylib", CallingConvention = (CallingConvention) 2)]
    public static extern void UnloadModelAnimation(ModelAnimation anim);

    /// <summary>Unload animation array data</summary>
    [DllImport("raylib", CallingConvention = (CallingConvention) 2)]
    public static extern void UnloadModelAnimations(ModelAnimation[] animations, uint count);

    /// <summary>Check model animation skeleton match</summary>
    [DllImport("raylib", CallingConvention = (CallingConvention) 2)]
    public static extern CBool IsModelAnimationValid(Model model, ModelAnimation anim);

    /// <summary>Detect collision between two spheres</summary>
    [DllImport("raylib", CallingConvention = (CallingConvention) 2)]
    public static extern CBool CheckCollisionSpheres(
      Vector3 center1,
      float radius1,
      Vector3 center2,
      float radius2);

    /// <summary>Detect collision between two bounding boxes</summary>
    [DllImport("raylib", CallingConvention = (CallingConvention) 2)]
    public static extern CBool CheckCollisionBoxes(BoundingBox box1, BoundingBox box2);

    /// <summary>Detect collision between box and sphere</summary>
    [DllImport("raylib", CallingConvention = (CallingConvention) 2)]
    public static extern CBool CheckCollisionBoxSphere(
      BoundingBox box,
      Vector3 center,
      float radius);

    /// <summary>Detect collision between ray and sphere</summary>
    [DllImport("raylib", CallingConvention = (CallingConvention) 2)]
    public static extern RayCollision GetRayCollisionSphere(Ray ray, Vector3 center, float radius);

    /// <summary>Detect collision between ray and box</summary>
    [DllImport("raylib", CallingConvention = (CallingConvention) 2)]
    public static extern RayCollision GetRayCollisionBox(Ray ray, BoundingBox box);

    /// <summary>Get collision info between ray and mesh</summary>
    [DllImport("raylib", CallingConvention = (CallingConvention) 2)]
    public static extern RayCollision GetRayCollisionMesh(Ray ray, Mesh mesh, Matrix4x4 transform);

    /// <summary>Get collision info between ray and triangle</summary>
    [DllImport("raylib", CallingConvention = (CallingConvention) 2)]
    public static extern RayCollision GetRayCollisionTriangle(
      Ray ray,
      Vector3 p1,
      Vector3 p2,
      Vector3 p3);

    /// <summary>Get collision info between ray and quad</summary>
    [DllImport("raylib", CallingConvention = (CallingConvention) 2)]
    public static extern RayCollision GetRayCollisionQuad(
      Ray ray,
      Vector3 p1,
      Vector3 p2,
      Vector3 p3,
      Vector3 p4);

    /// <summary>Initialize audio device and context</summary>
    [DllImport("raylib", CallingConvention = (CallingConvention) 2)]
    public static extern void InitAudioDevice();

    /// <summary>Close the audio device and context</summary>
    [DllImport("raylib", CallingConvention = (CallingConvention) 2)]
    public static extern void CloseAudioDevice();

    /// <summary>Check if audio device has been initialized successfully</summary>
    [DllImport("raylib", CallingConvention = (CallingConvention) 2)]
    public static extern CBool IsAudioDeviceReady();

    /// <summary>Set master volume (listener)</summary>
    [DllImport("raylib", CallingConvention = (CallingConvention) 2)]
    public static extern void SetMasterVolume(float volume);

    /// <summary>Load wave data from file</summary>
    [DllImport("raylib", CallingConvention = (CallingConvention) 2)]
    public static extern unsafe Wave LoadWave(sbyte* fileName);

    /// <summary>Load wave from memory buffer, fileType refers to extension: i.e. "wav"</summary>
    [DllImport("raylib", CallingConvention = (CallingConvention) 2)]
    public static extern unsafe Wave LoadWaveFromMemory(
      sbyte* fileType,
      byte* fileData,
      int dataSize);

    /// <summary>Checks if wave data is ready</summary>
    [DllImport("raylib", CallingConvention = (CallingConvention) 2)]
    public static extern CBool IsWaveReady(Wave wave);

    /// <summary>Load sound from file</summary>
    [DllImport("raylib", CallingConvention = (CallingConvention) 2)]
    public static extern unsafe Sound LoadSound(sbyte* fileName);

    /// <summary>Load sound from wave data</summary>
    [DllImport("raylib", CallingConvention = (CallingConvention) 2)]
    public static extern Sound LoadSoundFromWave(Wave wave);

    /// <summary>Checks if a sound is ready</summary>
    [DllImport("raylib", CallingConvention = (CallingConvention) 2)]
    public static extern CBool IsSoundReady(Sound sound);

    /// <summary>Update sound buffer with new data</summary>
    [DllImport("raylib", CallingConvention = (CallingConvention) 2)]
    public static extern unsafe void UpdateSound(Sound sound, void* data, int sampleCount);

    /// <summary>Unload wave data</summary>
    [DllImport("raylib", CallingConvention = (CallingConvention) 2)]
    public static extern void UnloadWave(Wave wave);

    /// <summary>Unload sound</summary>
    [DllImport("raylib", CallingConvention = (CallingConvention) 2)]
    public static extern void UnloadSound(Sound sound);

    /// <summary>Export wave data to file</summary>
    [DllImport("raylib", CallingConvention = (CallingConvention) 2)]
    public static extern unsafe CBool ExportWave(Wave wave, sbyte* fileName);

    /// <summary>Export wave sample data to code (.h)</summary>
    [DllImport("raylib", CallingConvention = (CallingConvention) 2)]
    public static extern unsafe CBool ExportWaveAsCode(Wave wave, sbyte* fileName);

    /// <summary>Play a sound</summary>
    [DllImport("raylib", CallingConvention = (CallingConvention) 2)]
    public static extern void PlaySound(Sound sound);

    /// <summary>Stop playing a sound</summary>
    [DllImport("raylib", CallingConvention = (CallingConvention) 2)]
    public static extern void StopSound(Sound sound);

    /// <summary>Pause a sound</summary>
    [DllImport("raylib", CallingConvention = (CallingConvention) 2)]
    public static extern void PauseSound(Sound sound);

    /// <summary>Resume a paused sound</summary>
    [DllImport("raylib", CallingConvention = (CallingConvention) 2)]
    public static extern void ResumeSound(Sound sound);

    /// <summary>Get number of sounds playing in the multichannel</summary>
    [DllImport("raylib", CallingConvention = (CallingConvention) 2)]
    public static extern int GetSoundsPlaying();

    /// <summary>Check if a sound is currently playing</summary>
    [DllImport("raylib", CallingConvention = (CallingConvention) 2)]
    public static extern CBool IsSoundPlaying(Sound sound);

    /// <summary>Set volume for a sound (1.0 is max level)</summary>
    [DllImport("raylib", CallingConvention = (CallingConvention) 2)]
    public static extern void SetSoundVolume(Sound sound, float volume);

    /// <summary>Set pitch for a sound (1.0 is base level)</summary>
    [DllImport("raylib", CallingConvention = (CallingConvention) 2)]
    public static extern void SetSoundPitch(Sound sound, float pitch);

    /// <summary>Set pan for a sound (0.5 is center)</summary>
    [DllImport("raylib", CallingConvention = (CallingConvention) 2)]
    public static extern void SetSoundPan(Sound sound, float pan);

    /// <summary>Copy a wave to a new wave</summary>
    [DllImport("raylib", CallingConvention = (CallingConvention) 2)]
    public static extern Wave WaveCopy(Wave wave);

    /// <summary>Crop a wave to defined samples range</summary>
    [DllImport("raylib", CallingConvention = (CallingConvention) 2)]
    public static extern unsafe void WaveCrop(Wave* wave, int initSample, int finalSample);

    /// <summary>Convert wave data to desired format</summary>
    [DllImport("raylib", CallingConvention = (CallingConvention) 2)]
    public static extern unsafe void WaveFormat(
      Wave* wave,
      int sampleRate,
      int sampleSize,
      int channels);

    /// <summary>Get samples data from wave as a floats array</summary>
    [DllImport("raylib", CallingConvention = (CallingConvention) 2)]
    public static extern unsafe float* LoadWaveSamples(Wave wave);

    /// <summary>Unload samples data loaded with LoadWaveSamples()</summary>
    [DllImport("raylib", CallingConvention = (CallingConvention) 2)]
    public static extern unsafe void UnloadWaveSamples(float* samples);

    /// <summary>Load music stream from file</summary>
    [DllImport("raylib", CallingConvention = (CallingConvention) 2)]
    public static extern unsafe Music LoadMusicStream(sbyte* fileName);

    /// <summary>Load music stream from data</summary>
    [DllImport("raylib", CallingConvention = (CallingConvention) 2)]
    public static extern unsafe Music LoadMusicStreamFromMemory(
      sbyte* fileType,
      byte* data,
      int dataSize);

    /// <summary>Checks if a music stream is ready</summary>
    [DllImport("raylib", CallingConvention = (CallingConvention) 2)]
    public static extern CBool IsMusicReady(Music music);

    /// <summary>Unload music stream</summary>
    [DllImport("raylib", CallingConvention = (CallingConvention) 2)]
    public static extern void UnloadMusicStream(Music music);

    /// <summary>Start music playing</summary>
    [DllImport("raylib", CallingConvention = (CallingConvention) 2)]
    public static extern void PlayMusicStream(Music music);

    /// <summary>Check if music is playing</summary>
    [DllImport("raylib", CallingConvention = (CallingConvention) 2)]
    public static extern CBool IsMusicStreamPlaying(Music music);

    /// <summary>Updates buffers for music streaming</summary>
    [DllImport("raylib", CallingConvention = (CallingConvention) 2)]
    public static extern void UpdateMusicStream(Music music);

    /// <summary>Stop music playing</summary>
    [DllImport("raylib", CallingConvention = (CallingConvention) 2)]
    public static extern void StopMusicStream(Music music);

    /// <summary>Pause music playing</summary>
    [DllImport("raylib", CallingConvention = (CallingConvention) 2)]
    public static extern void PauseMusicStream(Music music);

    /// <summary>Resume playing paused music</summary>
    [DllImport("raylib", CallingConvention = (CallingConvention) 2)]
    public static extern void ResumeMusicStream(Music music);

    /// <summary>Seek music to a position (in seconds)</summary>
    [DllImport("raylib", CallingConvention = (CallingConvention) 2)]
    public static extern void SeekMusicStream(Music music, float position);

    /// <summary>Set volume for music (1.0 is max level)</summary>
    [DllImport("raylib", CallingConvention = (CallingConvention) 2)]
    public static extern void SetMusicVolume(Music music, float volume);

    /// <summary>Set pitch for a music (1.0 is base level)</summary>
    [DllImport("raylib", CallingConvention = (CallingConvention) 2)]
    public static extern void SetMusicPitch(Music music, float pitch);

    /// <summary>Set pan for a music (0.5 is center)</summary>
    [DllImport("raylib", CallingConvention = (CallingConvention) 2)]
    public static extern void SetMusicPan(Music music, float pan);

    /// <summary>Get music time length (in seconds)</summary>
    [DllImport("raylib", CallingConvention = (CallingConvention) 2)]
    public static extern float GetMusicTimeLength(Music music);

    /// <summary>Get current music time played (in seconds)</summary>
    [DllImport("raylib", CallingConvention = (CallingConvention) 2)]
    public static extern float GetMusicTimePlayed(Music music);

    /// <summary>Init audio stream (to stream raw audio pcm data)</summary>
    [DllImport("raylib", CallingConvention = (CallingConvention) 2)]
    public static extern AudioStream LoadAudioStream(
      uint sampleRate,
      uint sampleSize,
      uint channels);

    /// <summary>Checks if an audio stream is ready</summary>
    [DllImport("raylib", CallingConvention = (CallingConvention) 2)]
    public static extern CBool IsAudioStreamReady(AudioStream stream);

    /// <summary>Unload audio stream and free memory</summary>
    [DllImport("raylib", CallingConvention = (CallingConvention) 2)]
    public static extern void UnloadAudioStream(AudioStream stream);

    /// <summary>Update audio stream buffers with data</summary>
    [DllImport("raylib", CallingConvention = (CallingConvention) 2)]
    public static extern unsafe void UpdateAudioStream(
      AudioStream stream,
      void* data,
      int frameCount);

    /// <summary>Check if any audio stream buffers requires refill</summary>
    [DllImport("raylib", CallingConvention = (CallingConvention) 2)]
    public static extern CBool IsAudioStreamProcessed(AudioStream stream);

    /// <summary>Play audio stream</summary>
    [DllImport("raylib", CallingConvention = (CallingConvention) 2)]
    public static extern void PlayAudioStream(AudioStream stream);

    /// <summary>Pause audio stream</summary>
    [DllImport("raylib", CallingConvention = (CallingConvention) 2)]
    public static extern void PauseAudioStream(AudioStream stream);

    /// <summary>Resume audio stream</summary>
    [DllImport("raylib", CallingConvention = (CallingConvention) 2)]
    public static extern void ResumeAudioStream(AudioStream stream);

    /// <summary>Check if audio stream is playing</summary>
    [DllImport("raylib", CallingConvention = (CallingConvention) 2)]
    public static extern CBool IsAudioStreamPlaying(AudioStream stream);

    /// <summary>Stop audio stream</summary>
    [DllImport("raylib", CallingConvention = (CallingConvention) 2)]
    public static extern void StopAudioStream(AudioStream stream);

    /// <summary>Set volume for audio stream (1.0 is max level)</summary>
    [DllImport("raylib", CallingConvention = (CallingConvention) 2)]
    public static extern void SetAudioStreamVolume(AudioStream stream, float volume);

    /// <summary>Set pitch for audio stream (1.0 is base level)</summary>
    [DllImport("raylib", CallingConvention = (CallingConvention) 2)]
    public static extern void SetAudioStreamPitch(AudioStream stream, float pitch);

    /// <summary>Set pan for audio stream (0.5 is centered)</summary>
    [DllImport("raylib", CallingConvention = (CallingConvention) 2)]
    public static extern void SetAudioStreamPan(AudioStream stream, float pan);

    /// <summary>Default size for new audio streams</summary>
    [DllImport("raylib", CallingConvention = (CallingConvention) 2)]
    public static extern void SetAudioStreamBufferSizeDefault(int size);

    /// <summary>Audio thread callback to request new data</summary>
    [DllImport("raylib", CallingConvention = (CallingConvention) 2)]
    public static extern void SetAudioStreamCallback(
      AudioStream stream,
      __FnPtr<void (void*, uint)> callback);

    /// <summary>Attach audio stream processor to stream</summary>
    [DllImport("raylib", CallingConvention = (CallingConvention) 2)]
    public static extern void AttachAudioStreamProcessor(
      AudioStream stream,
      __FnPtr<void (void*, uint)> processor);

    /// <summary>Detach audio stream processor from stream</summary>
    [DllImport("raylib", CallingConvention = (CallingConvention) 2)]
    public static extern void DetachAudioStreamProcessor(
      AudioStream stream,
      __FnPtr<void (void*, uint)> processor);

    /// <summary>Attach audio stream processor to the entire audio pipeline</summary>
    [DllImport("raylib", CallingConvention = (CallingConvention) 2)]
    public static extern void AttachAudioMixedProcessor(__FnPtr<void (void*, uint)> processor);

    /// <summary>Detach audio stream processor from the entire audio pipeline</summary>
    [DllImport("raylib", CallingConvention = (CallingConvention) 2)]
    public static extern void DetachAudioMixedProcessor(__FnPtr<void (void*, uint)> processor);

    /// <summary>Initialize window and OpenGL context</summary>
    public static unsafe void InitWindow(int width, int height, string title)
    {
      UTF8Buffer utF8Buffer = title.ToUTF8Buffer();
      try
      {
        Raylib.InitWindow(width, height, utF8Buffer.AsPointer());
      }
      finally
      {
        utF8Buffer.Dispose();
      }
    }

    /// <summary>Set title for window (only PLATFORM_DESKTOP)</summary>
    public static unsafe void SetWindowTitle(string title)
    {
      UTF8Buffer utF8Buffer = title.ToUTF8Buffer();
      try
      {
        Raylib.SetWindowTitle(utF8Buffer.AsPointer());
      }
      finally
      {
        utF8Buffer.Dispose();
      }
    }

    /// <summary>Get the human-readable, UTF-8 encoded name of the specified monitor</summary>
    public static unsafe string GetMonitorName_(int monitor) => Utf8StringUtils.GetUTF8String(Raylib.GetMonitorName(monitor));

    /// <summary>Get clipboard text content</summary>
    public static unsafe string GetClipboardText_() => Utf8StringUtils.GetUTF8String(Raylib.GetClipboardText());

    /// <summary>Set clipboard text content</summary>
    public static unsafe void SetClipboardText(string text)
    {
      UTF8Buffer utF8Buffer = text.ToUTF8Buffer();
      try
      {
        Raylib.SetClipboardText(utF8Buffer.AsPointer());
      }
      finally
      {
        utF8Buffer.Dispose();
      }
    }

    /// <summary>Load shader from files and bind default locations</summary>
    public static unsafe Shader LoadShader(string vsFileName, string fsFileName)
    {
      UTF8Buffer utF8Buffer1 = vsFileName.ToUTF8Buffer();
      try
      {
        UTF8Buffer utF8Buffer2 = fsFileName.ToUTF8Buffer();
        try
        {
          return Raylib.LoadShader(utF8Buffer1.AsPointer(), utF8Buffer2.AsPointer());
        }
        finally
        {
          utF8Buffer2.Dispose();
        }
      }
      finally
      {
        utF8Buffer1.Dispose();
      }
    }

    /// <summary>Load shader from code string and bind default locations</summary>
    public static unsafe Shader LoadShaderFromMemory(string vsCode, string fsCode)
    {
      UTF8Buffer utF8Buffer1 = vsCode.ToUTF8Buffer();
      try
      {
        UTF8Buffer utF8Buffer2 = fsCode.ToUTF8Buffer();
        try
        {
          return Raylib.LoadShaderFromMemory(utF8Buffer1.AsPointer(), utF8Buffer2.AsPointer());
        }
        finally
        {
          utF8Buffer2.Dispose();
        }
      }
      finally
      {
        utF8Buffer1.Dispose();
      }
    }

    /// <summary>Get shader uniform location</summary>
    public static unsafe int GetShaderLocation(Shader shader, string uniformName)
    {
      UTF8Buffer utF8Buffer = uniformName.ToUTF8Buffer();
      try
      {
        return Raylib.GetShaderLocation(shader, utF8Buffer.AsPointer());
      }
      finally
      {
        utF8Buffer.Dispose();
      }
    }

    /// <summary>Get shader attribute location</summary>
    public static unsafe int GetShaderLocationAttrib(Shader shader, string attribName)
    {
      UTF8Buffer utF8Buffer = attribName.ToUTF8Buffer();
      try
      {
        return Raylib.GetShaderLocationAttrib(shader, utF8Buffer.AsPointer());
      }
      finally
      {
        utF8Buffer.Dispose();
      }
    }

    /// <summary>Takes a screenshot of current screen (saved a .png)</summary>
    public static unsafe void TakeScreenshot(string fileName)
    {
      UTF8Buffer utF8Buffer = fileName.ToUTF8Buffer();
      try
      {
        Raylib.TakeScreenshot(utF8Buffer.AsPointer());
      }
      finally
      {
        utF8Buffer.Dispose();
      }
    }

    /// <summary>Check file extension</summary>
    public static unsafe CBool IsFileExtension(string fileName, string ext)
    {
      UTF8Buffer utF8Buffer1 = fileName.ToUTF8Buffer();
      try
      {
        UTF8Buffer utF8Buffer2 = ext.ToUTF8Buffer();
        try
        {
          return Raylib.IsFileExtension(utF8Buffer1.AsPointer(), utF8Buffer2.AsPointer());
        }
        finally
        {
          utF8Buffer2.Dispose();
        }
      }
      finally
      {
        utF8Buffer1.Dispose();
      }
    }

    /// <summary>Get file modification time (last write time)</summary>
    public static unsafe long GetFileModTime(string fileName)
    {
      UTF8Buffer utF8Buffer = fileName.ToUTF8Buffer();
      try
      {
        return (long) Raylib.GetFileModTime(utF8Buffer.AsPointer());
      }
      finally
      {
        utF8Buffer.Dispose();
      }
    }

    /// <summary>Load image from file into CPU memory (RAM)</summary>
    public static unsafe Image LoadImage(string fileName)
    {
      UTF8Buffer utF8Buffer = fileName.ToUTF8Buffer();
      try
      {
        return Raylib.LoadImage(utF8Buffer.AsPointer());
      }
      finally
      {
        utF8Buffer.Dispose();
      }
    }

    /// <summary>Load image from RAW file data</summary>
    public static unsafe Image LoadImageRaw(
      string fileName,
      int width,
      int height,
      PixelFormat format,
      int headerSize)
    {
      UTF8Buffer utF8Buffer = fileName.ToUTF8Buffer();
      try
      {
        return Raylib.LoadImageRaw(utF8Buffer.AsPointer(), width, height, format, headerSize);
      }
      finally
      {
        utF8Buffer.Dispose();
      }
    }

    /// <summary>Load image sequence from file (frames appended to image.data)</summary>
    public static unsafe Image LoadImageAnim(string fileName, out int frames)
    {
      UTF8Buffer utF8Buffer = fileName.ToUTF8Buffer();
      try
      {
        fixed (int* frames1 = &frames)
          return Raylib.LoadImageAnim(utF8Buffer.AsPointer(), frames1);
      }
      finally
      {
        utF8Buffer.Dispose();
      }
    }

    /// <summary>Export image data to file</summary>
    public static unsafe CBool ExportImage(Image image, string fileName)
    {
      UTF8Buffer utF8Buffer = fileName.ToUTF8Buffer();
      try
      {
        return Raylib.ExportImage(image, utF8Buffer.AsPointer());
      }
      finally
      {
        utF8Buffer.Dispose();
      }
    }

    /// <summary>Export image as code file defining an array of bytes</summary>
    public static unsafe CBool ExportImageAsCode(Image image, string fileName)
    {
      UTF8Buffer utF8Buffer = fileName.ToUTF8Buffer();
      try
      {
        return Raylib.ExportImageAsCode(image, utF8Buffer.AsPointer());
      }
      finally
      {
        utF8Buffer.Dispose();
      }
    }

    /// <summary>Show trace log messages (LOG_DEBUG, LOG_INFO, LOG_WARNING, LOG_ERROR)</summary>
    public static unsafe void TraceLog(TraceLogLevel logLevel, string text)
    {
      UTF8Buffer utF8Buffer = text.ToUTF8Buffer();
      try
      {
        Raylib.TraceLog(logLevel, utF8Buffer.AsPointer());
      }
      finally
      {
        utF8Buffer.Dispose();
      }
    }

    /// <summary>Set shader uniform value vector</summary>
    public static void SetShaderValueV<T>(
      Shader shader,
      int locIndex,
      T[] values,
      ShaderUniformDataType uniformType,
      int count)
      where T : unmanaged
    {
      Raylib.SetShaderValueV<T>(shader, locIndex, (Span<T>) values, uniformType, count);
    }

    /// <summary>Set shader uniform value vector</summary>
    public static unsafe void SetShaderValueV<T>(
      Shader shader,
      int locIndex,
      Span<T> values,
      ShaderUniformDataType uniformType,
      int count)
      where T : unmanaged
    {
      fixed (T* objPtr = &values.GetPinnableReference())
        Raylib.SetShaderValueV(shader, locIndex, (void*) objPtr, uniformType, count);
    }

    /// <summary>Set shader uniform value</summary>
    public static unsafe void SetShaderValue<T>(
      Shader shader,
      int locIndex,
      T value,
      ShaderUniformDataType uniformType)
      where T : unmanaged
    {
      Raylib.SetShaderValue(shader, locIndex, (void*) &value, uniformType);
    }

    /// <summary>Set shader uniform value</summary>
    public static void SetShaderValue<T>(
      Shader shader,
      int locIndex,
      T[] values,
      ShaderUniformDataType uniformType)
      where T : unmanaged
    {
      Raylib.SetShaderValue<T>(shader, locIndex, (Span<T>) values, uniformType);
    }

    /// <summary>Set shader uniform value</summary>
    public static unsafe void SetShaderValue<T>(
      Shader shader,
      int locIndex,
      Span<T> values,
      ShaderUniformDataType uniformType)
      where T : unmanaged
    {
      fixed (T* objPtr = &values.GetPinnableReference())
        Raylib.SetShaderValue(shader, locIndex, (void*) objPtr, uniformType);
    }

    /// <summary>Load file data as byte array (read)</summary>
    public static unsafe byte* LoadFileData(string fileName, ref uint bytesRead)
    {
      UTF8Buffer utF8Buffer = fileName.ToUTF8Buffer();
      try
      {
        fixed (uint* bytesRead1 = &bytesRead)
          return Raylib.LoadFileData(utF8Buffer.AsPointer(), bytesRead1);
      }
      finally
      {
        utF8Buffer.Dispose();
      }
    }

    /// <summary>Get dropped files names (memory should be freed)</summary>
    public static unsafe string[] GetDroppedFiles()
    {
      FilePathList files = Raylib.LoadDroppedFiles();
      string[] droppedFiles = new string[(int) files.count];
      for (int index = 0; (long) index < (long) files.count; ++index)
        droppedFiles[index] = Marshal.PtrToStringUTF8((IntPtr) (void*) files.paths[index]);
      Raylib.UnloadDroppedFiles(files);
      return droppedFiles;
    }

    /// <summary>Get gamepad internal name id</summary>
    public static unsafe string GetGamepadName_(int gamepad) => Utf8StringUtils.GetUTF8String(Raylib.GetGamepadName(gamepad));

    /// <summary>Update camera position for selected mode</summary>
    public static unsafe void UpdateCamera(ref Camera3D camera, CameraMode mode)
    {
      fixed (Camera3D* camera1 = &camera)
        Raylib.UpdateCamera(camera1, mode);
    }

    /// <summary>Update camera movement/rotation</summary>
    public static unsafe void UpdateCameraPro(
      ref Camera3D camera,
      Vector3 movement,
      Vector3 rotation,
      float zoom)
    {
      fixed (Camera3D* camera1 = &camera)
        Raylib.UpdateCameraPro(camera1, movement, rotation, zoom);
    }

    /// <summary>
    /// Check the collision between two lines defined by two points each, returns collision point by reference
    /// </summary>
    public static unsafe CBool CheckCollisionLines(
      Vector2 startPos1,
      Vector2 endPos1,
      Vector2 startPos2,
      Vector2 endPos2,
      ref Vector2 collisionPoint)
    {
      fixed (Vector2* collisionPoint1 = &collisionPoint)
        return Raylib.CheckCollisionLines(startPos1, endPos1, startPos2, endPos2, collisionPoint1);
    }

    /// <summary>Create an image from text (default font)</summary>
    public static unsafe Image ImageText(string text, int fontSize, Color color)
    {
      UTF8Buffer utF8Buffer = text.ToUTF8Buffer();
      try
      {
        return Raylib.ImageText(utF8Buffer.AsPointer(), fontSize, color);
      }
      finally
      {
        utF8Buffer.Dispose();
      }
    }

    /// <summary>Create an image from text (custom sprite font)</summary>
    public static unsafe Image ImageTextEx(
      Font font,
      string text,
      float fontSize,
      float spacing,
      Color tint)
    {
      UTF8Buffer utF8Buffer = text.ToUTF8Buffer();
      try
      {
        return Raylib.ImageTextEx(font, utF8Buffer.AsPointer(), fontSize, spacing, tint);
      }
      finally
      {
        utF8Buffer.Dispose();
      }
    }

    /// <summary>Convert image to POT (power-of-two)</summary>
    public static unsafe void ImageToPOT(ref Image image, Color fill)
    {
      fixed (Image* image1 = &image)
        Raylib.ImageToPOT(image1, fill);
    }

    /// <summary>Convert image data to desired format</summary>
    public static unsafe void ImageFormat(ref Image image, PixelFormat newFormat)
    {
      fixed (Image* image1 = &image)
        Raylib.ImageFormat(image1, newFormat);
    }

    /// <summary>Apply alpha mask to image</summary>
    public static unsafe void ImageAlphaMask(ref Image image, Image alphaMask)
    {
      fixed (Image* image1 = &image)
        Raylib.ImageAlphaMask(image1, alphaMask);
    }

    /// <summary>Clear alpha channel to desired color</summary>
    public static unsafe void ImageAlphaClear(ref Image image, Color color, float threshold)
    {
      fixed (Image* image1 = &image)
        Raylib.ImageAlphaClear(image1, color, threshold);
    }

    /// <summary>Crop image depending on alpha value</summary>
    public static unsafe void ImageAlphaCrop(ref Image image, float threshold)
    {
      fixed (Image* image1 = &image)
        Raylib.ImageAlphaCrop(image1, threshold);
    }

    /// <summary>Premultiply alpha channel</summary>
    public static unsafe void ImageAlphaPremultiply(ref Image image)
    {
      fixed (Image* image1 = &image)
        Raylib.ImageAlphaPremultiply(image1);
    }

    /// <summary>Crop an image to a defined rectangle</summary>
    public static unsafe void ImageCrop(ref Image image, Rectangle crop)
    {
      fixed (Image* image1 = &image)
        Raylib.ImageCrop(image1, crop);
    }

    /// <summary>Resize image (Bicubic scaling algorithm)</summary>
    public static unsafe void ImageResize(ref Image image, int newWidth, int newHeight)
    {
      fixed (Image* image1 = &image)
        Raylib.ImageResize(image1, newWidth, newHeight);
    }

    /// <summary>Resize image (Nearest-Neighbor scaling algorithm)</summary>
    public static unsafe void ImageResizeNN(ref Image image, int newWidth, int newHeight)
    {
      fixed (Image* image1 = &image)
        Raylib.ImageResizeNN(image1, newWidth, newHeight);
    }

    /// <summary>Resize canvas and fill with color</summary>
    public static unsafe void ImageResizeCanvas(
      ref Image image,
      int newWidth,
      int newHeight,
      int offsetX,
      int offsetY,
      Color color)
    {
      fixed (Image* image1 = &image)
        Raylib.ImageResizeCanvas(image1, newWidth, newHeight, offsetX, offsetY, color);
    }

    /// <summary>Generate all mipmap levels for a provided image</summary>
    public static unsafe void ImageMipmaps(ref Image image)
    {
      fixed (Image* image1 = &image)
        Raylib.ImageMipmaps(image1);
    }

    /// <summary>Dither image data to 16bpp or lower (Floyd-Steinberg dithering)</summary>
    public static unsafe void ImageDither(
      ref Image image,
      int rBpp,
      int gBpp,
      int bBpp,
      int aBpp)
    {
      fixed (Image* image1 = &image)
        Raylib.ImageDither(image1, rBpp, gBpp, bBpp, aBpp);
    }

    /// <summary>Flip image vertically</summary>
    public static unsafe void ImageFlipVertical(ref Image image)
    {
      fixed (Image* image1 = &image)
        Raylib.ImageFlipVertical(image1);
    }

    /// <summary>Flip image horizontally</summary>
    public static unsafe void ImageFlipHorizontal(ref Image image)
    {
      fixed (Image* image1 = &image)
        Raylib.ImageFlipHorizontal(image1);
    }

    /// <summary>Rotate image clockwise 90deg</summary>
    public static unsafe void ImageRotateCW(ref Image image)
    {
      fixed (Image* image1 = &image)
        Raylib.ImageRotateCW(image1);
    }

    /// <summary>Rotate image counter-clockwise 90deg</summary>
    public static unsafe void ImageRotateCCW(ref Image image)
    {
      fixed (Image* image1 = &image)
        Raylib.ImageRotateCCW(image1);
    }

    /// <summary>Modify image color: tint</summary>
    public static unsafe void ImageColorTint(ref Image image, Color color)
    {
      fixed (Image* image1 = &image)
        Raylib.ImageColorTint(image1, color);
    }

    /// <summary>Modify image color: invert</summary>
    public static unsafe void ImageColorInvert(ref Image image)
    {
      fixed (Image* image1 = &image)
        Raylib.ImageColorInvert(image1);
    }

    /// <summary>Modify image color: grayscale</summary>
    public static unsafe void ImageColorGrayscale(ref Image image)
    {
      fixed (Image* image1 = &image)
        Raylib.ImageColorGrayscale(image1);
    }

    /// <summary>Modify image color: contrast (-100 to 100)</summary>
    public static unsafe void ImageColorContrast(ref Image image, float contrast)
    {
      fixed (Image* image1 = &image)
        Raylib.ImageColorContrast(image1, contrast);
    }

    /// <summary>Modify image color: brightness (-255 to 255)</summary>
    public static unsafe void ImageColorBrightness(ref Image image, int brightness)
    {
      fixed (Image* image1 = &image)
        Raylib.ImageColorBrightness(image1, brightness);
    }

    /// <summary>Modify image color: replace color</summary>
    public static unsafe void ImageColorReplace(ref Image image, Color color, Color replace)
    {
      fixed (Image* image1 = &image)
        Raylib.ImageColorReplace(image1, color, replace);
    }

    /// <summary>Clear image background with given color</summary>
    public static unsafe void ImageClearBackground(ref Image dst, Color color)
    {
      fixed (Image* dst1 = &dst)
        Raylib.ImageClearBackground(dst1, color);
    }

    /// <summary>Draw pixel within an image</summary>
    public static unsafe void ImageDrawPixel(ref Image dst, int posX, int posY, Color color)
    {
      fixed (Image* dst1 = &dst)
        Raylib.ImageDrawPixel(dst1, posX, posY, color);
    }

    /// <summary>Draw pixel within an image (Vector version)</summary>
    public static unsafe void ImageDrawPixelV(ref Image dst, Vector2 position, Color color)
    {
      fixed (Image* dst1 = &dst)
        Raylib.ImageDrawPixelV(dst1, position, color);
    }

    /// <summary>Draw line within an image</summary>
    public static unsafe void ImageDrawLine(
      ref Image dst,
      int startPosX,
      int startPosY,
      int endPosX,
      int endPosY,
      Color color)
    {
      fixed (Image* dst1 = &dst)
        Raylib.ImageDrawLine(dst1, startPosX, startPosY, endPosX, endPosY, color);
    }

    /// <summary>Draw line within an image (Vector version)</summary>
    public static unsafe void ImageDrawLineV(
      ref Image dst,
      Vector2 start,
      Vector2 end,
      Color color)
    {
      fixed (Image* dst1 = &dst)
        Raylib.ImageDrawLineV(dst1, start, end, color);
    }

    /// <summary>Draw circle within an image</summary>
    public static unsafe void ImageDrawCircle(
      ref Image dst,
      int centerX,
      int centerY,
      int radius,
      Color color)
    {
      fixed (Image* dst1 = &dst)
        Raylib.ImageDrawCircle(dst1, centerX, centerY, radius, color);
    }

    /// <summary>Draw circle within an image (Vector version)</summary>
    public static unsafe void ImageDrawCircleV(
      ref Image dst,
      Vector2 center,
      int radius,
      Color color)
    {
      fixed (Image* dst1 = &dst)
        Raylib.ImageDrawCircleV(dst1, center, radius, color);
    }

    /// <summary>Draw rectangle within an image</summary>
    public static unsafe void ImageDrawRectangle(
      ref Image dst,
      int posX,
      int posY,
      int width,
      int height,
      Color color)
    {
      fixed (Image* dst1 = &dst)
        Raylib.ImageDrawRectangle(dst1, posX, posY, width, height, color);
    }

    /// <summary>Draw rectangle within an image (Vector version)</summary>
    public static unsafe void ImageDrawRectangleV(
      ref Image dst,
      Vector2 position,
      Vector2 size,
      Color color)
    {
      fixed (Image* dst1 = &dst)
        Raylib.ImageDrawRectangleV(dst1, position, size, color);
    }

    /// <summary>Draw rectangle within an image</summary>
    public static unsafe void ImageDrawRectangleRec(ref Image dst, Rectangle rec, Color color)
    {
      fixed (Image* dst1 = &dst)
        Raylib.ImageDrawRectangleRec(dst1, rec, color);
    }

    /// <summary>Draw rectangle lines within an image</summary>
    public static unsafe void ImageDrawRectangleLines(
      ref Image dst,
      Rectangle rec,
      int thick,
      Color color)
    {
      fixed (Image* dst1 = &dst)
        Raylib.ImageDrawRectangleLines(dst1, rec, thick, color);
    }

    /// <summary>Draw a source image within a destination image (tint applied to source)</summary>
    public static unsafe void ImageDraw(
      ref Image dst,
      Image src,
      Rectangle srcRec,
      Rectangle dstRec,
      Color tint)
    {
      fixed (Image* dst1 = &dst)
        Raylib.ImageDraw(dst1, src, srcRec, dstRec, tint);
    }

    /// <summary>Draw text (using default font) within an image (destination)</summary>
    public static unsafe void ImageDrawText(
      ref Image dst,
      string text,
      int x,
      int y,
      int fontSize,
      Color color)
    {
      UTF8Buffer utF8Buffer = text.ToUTF8Buffer();
      try
      {
        fixed (Image* dst1 = &dst)
          Raylib.ImageDrawText(dst1, utF8Buffer.AsPointer(), x, y, fontSize, color);
      }
      finally
      {
        utF8Buffer.Dispose();
      }
    }

    /// <summary>Draw text (custom sprite font) within an image (destination)</summary>
    public static unsafe void ImageDrawTextEx(
      ref Image dst,
      Font font,
      string text,
      Vector2 position,
      int fontSize,
      float spacing,
      Color color)
    {
      UTF8Buffer utF8Buffer = text.ToUTF8Buffer();
      try
      {
        fixed (Image* dst1 = &dst)
          Raylib.ImageDrawTextEx(dst1, font, utF8Buffer.AsPointer(), position, (float) fontSize, spacing, color);
      }
      finally
      {
        utF8Buffer.Dispose();
      }
    }

    /// <summary>Load texture from file into GPU memory (VRAM)</summary>
    public static unsafe Texture2D LoadTexture(string fileName)
    {
      UTF8Buffer utF8Buffer = fileName.ToUTF8Buffer();
      try
      {
        return Raylib.LoadTexture(utF8Buffer.AsPointer());
      }
      finally
      {
        utF8Buffer.Dispose();
      }
    }

    /// <summary>Generate GPU mipmaps for a texture</summary>
    public static unsafe void GenTextureMipmaps(ref Texture2D texture)
    {
      fixed (Texture2D* texture1 = &texture)
        Raylib.GenTextureMipmaps(texture1);
    }

    /// <summary>Load font from file into GPU memory (VRAM)</summary>
    public static unsafe Font LoadFont(string fileName)
    {
      UTF8Buffer utF8Buffer = fileName.ToUTF8Buffer();
      try
      {
        return Raylib.LoadFont(utF8Buffer.AsPointer());
      }
      finally
      {
        utF8Buffer.Dispose();
      }
    }

    /// <summary>Load font from file with extended parameters</summary>
    public static unsafe Font LoadFontEx(
      string fileName,
      int fontSize,
      int[] fontChars,
      int glyphCount)
    {
      UTF8Buffer utF8Buffer = fileName.ToUTF8Buffer();
      try
      {
        fixed (int* fontChars1 = fontChars)
          return Raylib.LoadFontEx(utF8Buffer.AsPointer(), fontSize, fontChars1, glyphCount);
      }
      finally
      {
        utF8Buffer.Dispose();
      }
    }

    /// <summary>Upload vertex data into GPU and provided VAO/VBO ids</summary>
    public static unsafe void UploadMesh(ref Mesh mesh, CBool dynamic)
    {
      fixed (Mesh* mesh1 = &mesh)
        Raylib.UploadMesh(mesh1, dynamic);
    }

    /// <summary>Unload mesh from memory (RAM and/or VRAM)</summary>
    public static unsafe void UnloadMesh(ref Mesh mesh)
    {
      fixed (Mesh* mesh1 = &mesh)
        Raylib.UnloadMesh(mesh1);
    }

    /// <summary>Set texture for a material map type (MAP_DIFFUSE, MAP_SPECULAR...)</summary>
    public static unsafe void SetMaterialTexture(
      ref Material material,
      MaterialMapIndex mapType,
      Texture2D texture)
    {
      fixed (Material* material1 = &material)
        Raylib.SetMaterialTexture(material1, mapType, texture);
    }

    /// <summary>Set material for a mesh</summary>
    public static unsafe void SetModelMeshMaterial(ref Model model, int meshId, int materialId)
    {
      fixed (Model* model1 = &model)
        Raylib.SetModelMeshMaterial(model1, meshId, materialId);
    }

    /// <summary>Load model animations from file</summary>
    public static unsafe ReadOnlySpan<ModelAnimation> LoadModelAnimations(
      string fileName,
      ref uint animCount)
    {
      UTF8Buffer utF8Buffer = fileName.ToUTF8Buffer();
      try
      {
        fixed (uint* animCount1 = &animCount)
        {
          ModelAnimation* pointer = Raylib.LoadModelAnimations(utF8Buffer.AsPointer(), animCount1);
          if ((IntPtr) (void*) pointer == IntPtr.Zero)
            throw new ApplicationException("Failed to load animation");
          int length = (int) animCount;
          return new ReadOnlySpan<ModelAnimation>((void*) pointer, length);
        }
      }
      finally
      {
        utF8Buffer.Dispose();
      }
    }

    /// <summary>Compute mesh tangents</summary>
    public static unsafe void GenMeshTangents(ref Mesh mesh)
    {
      fixed (Mesh* mesh1 = &mesh)
        Raylib.GenMeshTangents(mesh1);
    }

    /// <summary>Convert wave data to desired format</summary>
    public static unsafe void WaveFormat(
      ref Wave wave,
      int sampleRate,
      int sampleSize,
      int channels)
    {
      fixed (Wave* wave1 = &wave)
        Raylib.WaveFormat(wave1, sampleRate, sampleSize, channels);
    }

    /// <summary>Crop a wave to defined samples range</summary>
    public static unsafe void WaveCrop(ref Wave wave, int initSample, int finalSample)
    {
      fixed (Wave* wave1 = &wave)
        Raylib.WaveCrop(wave1, initSample, finalSample);
    }

    /// <summary>Draw lines sequence</summary>
    public static unsafe void DrawLineStrip(Vector2[] points, int pointCount, Color color)
    {
      fixed (Vector2* points1 = points)
        Raylib.DrawLineStrip(points1, pointCount, color);
    }

    /// <summary>Draw a triangle fan defined by points (first vertex is the center)</summary>
    public static unsafe void DrawTriangleFan(Vector2[] points, int pointCount, Color color)
    {
      fixed (Vector2* points1 = points)
        Raylib.DrawTriangleFan(points1, pointCount, color);
    }

    /// <summary>Draw a triangle strip defined by points</summary>
    public static unsafe void DrawTriangleStrip(Vector2[] points, int pointCount, Color color)
    {
      fixed (Vector2* points1 = points)
        Raylib.DrawTriangleStrip(points1, pointCount, color);
    }

    /// <summary>Draw text (using default font)</summary>
    public static unsafe void DrawText(
      string text,
      int posX,
      int posY,
      int fontSize,
      Color color)
    {
      UTF8Buffer utF8Buffer = text.ToUTF8Buffer();
      try
      {
        Raylib.DrawText(utF8Buffer.AsPointer(), posX, posY, fontSize, color);
      }
      finally
      {
        utF8Buffer.Dispose();
      }
    }

    /// <summary>Draw text using font and additional parameters</summary>
    public static unsafe void DrawTextEx(
      Font font,
      string text,
      Vector2 position,
      float fontSize,
      float spacing,
      Color tint)
    {
      UTF8Buffer utF8Buffer = text.ToUTF8Buffer();
      try
      {
        Raylib.DrawTextEx(font, utF8Buffer.AsPointer(), position, fontSize, spacing, tint);
      }
      finally
      {
        utF8Buffer.Dispose();
      }
    }

    /// <summary>Draw text using Font and pro parameters (rotation)</summary>
    public static unsafe void DrawTextPro(
      Font font,
      string text,
      Vector2 position,
      Vector2 origin,
      float rotation,
      float fontSize,
      float spacing,
      Color tint)
    {
      UTF8Buffer utF8Buffer = text.ToUTF8Buffer();
      try
      {
        Raylib.DrawTextPro(font, utF8Buffer.AsPointer(), position, origin, rotation, fontSize, spacing, tint);
      }
      finally
      {
        utF8Buffer.Dispose();
      }
    }

    /// <summary>Measure string width for default font</summary>
    public static unsafe int MeasureText(string text, int fontSize)
    {
      UTF8Buffer utF8Buffer = text.ToUTF8Buffer();
      try
      {
        return Raylib.MeasureText(utF8Buffer.AsPointer(), fontSize);
      }
      finally
      {
        utF8Buffer.Dispose();
      }
    }

    /// <summary>Measure string size for Font</summary>
    public static unsafe Vector2 MeasureTextEx(
      Font font,
      string text,
      float fontSize,
      float spacing)
    {
      UTF8Buffer utF8Buffer = text.ToUTF8Buffer();
      try
      {
        return Raylib.MeasureTextEx(font, utF8Buffer.AsPointer(), fontSize, spacing);
      }
      finally
      {
        utF8Buffer.Dispose();
      }
    }

    /// <summary>Append text at specific position and move cursor!</summary>
    public static unsafe void TextAppend(string text, string append, int position)
    {
      UTF8Buffer utF8Buffer1 = text.ToUTF8Buffer();
      try
      {
        UTF8Buffer utF8Buffer2 = append.ToUTF8Buffer();
        try
        {
          Raylib.TextAppend(utF8Buffer1.AsPointer(), utF8Buffer2.AsPointer(), &position);
        }
        finally
        {
          utF8Buffer2.Dispose();
        }
      }
      finally
      {
        utF8Buffer1.Dispose();
      }
    }

    /// <summary>Get Pascal case notation version of provided string</summary>
    public static unsafe string TextToPascal(string text)
    {
      UTF8Buffer utF8Buffer = text.ToUTF8Buffer();
      try
      {
        return Utf8StringUtils.GetUTF8String(Raylib.TextToPascal(utF8Buffer.AsPointer()));
      }
      finally
      {
        utF8Buffer.Dispose();
      }
    }

    /// <summary>Get integer value from text (negative values not supported)</summary>
    public static unsafe int TextToInteger(string text)
    {
      UTF8Buffer utF8Buffer = text.ToUTF8Buffer();
      try
      {
        return Raylib.TextToInteger(utF8Buffer.AsPointer());
      }
      finally
      {
        utF8Buffer.Dispose();
      }
    }

    /// <summary>Get all codepoints in a string, codepoints count returned by parameters</summary>
    public static unsafe int[] LoadCodepoints(string text, ref int count)
    {
      UTF8Buffer utF8Buffer = text.ToUTF8Buffer();
      try
      {
        fixed (int* count1 = &count)
        {
          int* numPtr = Raylib.LoadCodepoints(utF8Buffer.AsPointer(), count1);
          int[] array = new ReadOnlySpan<int>((void*) numPtr, count).ToArray();
          Raylib.UnloadCodepoints(numPtr);
          return array;
        }
      }
      finally
      {
        utF8Buffer.Dispose();
      }
    }

    /// <summary>Get total number of codepoints in a UTF8 encoded string</summary>
    public static unsafe int GetCodepointCount(string text)
    {
      UTF8Buffer utF8Buffer = text.ToUTF8Buffer();
      try
      {
        return Raylib.GetCodepointCount(utF8Buffer.AsPointer());
      }
      finally
      {
        utF8Buffer.Dispose();
      }
    }

    /// <summary>Get next codepoint in a UTF-8 encoded string, 0x3f('?') is returned on failure</summary>
    /// <returns>single codepoint / "char"</returns>
    public static unsafe int GetCodepoint(string text, ref int codepointSize)
    {
      UTF8Buffer utF8Buffer = text.ToUTF8Buffer();
      try
      {
        fixed (int* codepointSize1 = &codepointSize)
          return Raylib.GetCodepointNext(utF8Buffer.AsPointer(), codepointSize1);
      }
      finally
      {
        utF8Buffer.Dispose();
      }
    }

    /// <summary>Encode one codepoint into UTF-8 byte array (array length returned as parameter)</summary>
    public static unsafe string CodepointToUTF8(int codepoint, ref int utf8Size)
    {
      fixed (int* utf8Size1 = &utf8Size)
        return Utf8StringUtils.GetUTF8String(Raylib.CodepointToUTF8(codepoint, utf8Size1));
    }

    /// <summary>Load UTF-8 text encoded from codepoints array</summary>
    public static unsafe string LoadUTF8(int[] codepoints, int length)
    {
      fixed (int* codepoints1 = codepoints)
      {
        sbyte* numPtr = Raylib.LoadUTF8(codepoints1, length);
        string utF8String = Utf8StringUtils.GetUTF8String(numPtr);
        Raylib.MemFree((void*) numPtr);
        return utF8String;
      }
    }

    /// <summary>Draw a model (with texture if set)</summary>
    public static unsafe Model LoadModel(string fileName)
    {
      UTF8Buffer utF8Buffer = fileName.ToUTF8Buffer();
      try
      {
        return Raylib.LoadModel(utF8Buffer.AsPointer());
      }
      finally
      {
        utF8Buffer.Dispose();
      }
    }

    /// <summary>Export mesh data to file, returns true on success</summary>
    public static unsafe CBool ExportMesh(Mesh mesh, string fileName)
    {
      UTF8Buffer utF8Buffer = fileName.ToUTF8Buffer();
      try
      {
        return Raylib.ExportMesh(mesh, utF8Buffer.AsPointer());
      }
      finally
      {
        utF8Buffer.Dispose();
      }
    }

    /// <summary>Draw a triangle strip defined by points</summary>
    public static unsafe void DrawTriangleStrip3D(Vector3[] points, int pointCount, Color color)
    {
      fixed (Vector3* points1 = points)
        Raylib.DrawTriangleStrip3D(points1, pointCount, color);
    }

    /// <summary>Draw multiple mesh instances with material and different transforms</summary>
    public static unsafe void DrawMeshInstanced(
      Mesh mesh,
      Material material,
      Matrix4x4[] transforms,
      int instances)
    {
      fixed (Matrix4x4* transforms1 = transforms)
        Raylib.DrawMeshInstanced(mesh, material, transforms1, instances);
    }

    /// <summary>Load wave data from file</summary>
    public static unsafe Wave LoadWave(string fileName)
    {
      UTF8Buffer utF8Buffer = fileName.ToUTF8Buffer();
      try
      {
        return Raylib.LoadWave(utF8Buffer.AsPointer());
      }
      finally
      {
        utF8Buffer.Dispose();
      }
    }

    /// <summary>Load sound from file</summary>
    public static unsafe Sound LoadSound(string fileName)
    {
      UTF8Buffer utF8Buffer = fileName.ToUTF8Buffer();
      try
      {
        return Raylib.LoadSound(utF8Buffer.AsPointer());
      }
      finally
      {
        utF8Buffer.Dispose();
      }
    }

    /// <summary>Export wave data to file</summary>
    public static unsafe CBool ExportWave(Wave wave, string fileName)
    {
      UTF8Buffer utF8Buffer = fileName.ToUTF8Buffer();
      try
      {
        return Raylib.ExportWave(wave, utF8Buffer.AsPointer());
      }
      finally
      {
        utF8Buffer.Dispose();
      }
    }

    /// <summary>Export wave sample data to code (.h)</summary>
    public static unsafe CBool ExportWaveAsCode(Wave wave, string fileName)
    {
      UTF8Buffer utF8Buffer = fileName.ToUTF8Buffer();
      try
      {
        return Raylib.ExportWaveAsCode(wave, utF8Buffer.AsPointer());
      }
      finally
      {
        utF8Buffer.Dispose();
      }
    }

    /// <summary>Load music stream from file</summary>
    public static unsafe Music LoadMusicStream(string fileName)
    {
      UTF8Buffer utF8Buffer = fileName.ToUTF8Buffer();
      try
      {
        return Raylib.LoadMusicStream(utF8Buffer.AsPointer());
      }
      finally
      {
        utF8Buffer.Dispose();
      }
    }

    public static string SubText(this string input, int position, int length) => input.Substring(position, Math.Min(length, input.Length));

    public static unsafe Material GetMaterial(ref Model model, int materialIndex) => model.materials[materialIndex];

    public static unsafe Texture2D GetMaterialTexture(
      ref Model model,
      int materialIndex,
      MaterialMapIndex mapIndex)
    {
      return ((MaterialMap*) ((IntPtr) model.materials[materialIndex].maps + (IntPtr) mapIndex * sizeof (MaterialMap)))->texture;
    }

    public static unsafe void SetMaterialTexture(
      ref Model model,
      int materialIndex,
      MaterialMapIndex mapIndex,
      ref Texture2D texture)
    {
      Raylib.SetMaterialTexture(model.materials + materialIndex, mapIndex, texture);
    }

    public static unsafe void SetMaterialShader(
      ref Model model,
      int materialIndex,
      ref Shader shader)
    {
      model.materials[materialIndex].shader = shader;
    }
  }
}
