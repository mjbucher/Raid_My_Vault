// MapTexNative.cpp : Defines the exported functions for the DLL application.
//
#if _MSC_VER // this is defined when compiling with Visual Studio
#define EXPORT_API __declspec(dllexport) // Visual Studio needs annotating exported functions with this
#else
//#define EXPORT_API // XCode does not need annotating exported functions, so define is empty
#endif

#include <CoreGraphics/CoreGraphics.h>

#include <OpenGLES/ES2/gl.h>
#include <OpenGLES/ES2/glext.h>
#include <stdio.h>

//#include <ApplicationServices/ApplicationServices.h>

unsigned char g_renderBuf[256 * 256 * 4];
bool g_initialized = false;
CGContextRef g_context;

extern "C"
{    
	float GetVal() { return (float)5.0; }

    void Initialize() {
        CGColorSpaceRef colorSpace = CGColorSpaceCreateDeviceRGB();
        
        g_context =
        CGBitmapContextCreate(g_renderBuf, 256, 256, 8, 256 * 4, colorSpace, kCGImageAlphaPremultipliedLast);
        
        CGContextTranslateCTM(g_context, 0, 256);
        CGContextScaleCTM(g_context, 1.0, -1.0);

        g_initialized = true;
        
        CGColorSpaceRelease(colorSpace);
    }
    
    int GetTextureFormat() {
        return 4;
    }
    
    int UpdateTileRaw(
        GLuint texId,
        int px,
        int py,
        int w,
        int h,
        unsigned char * buf,
        int offset) {
        
		glBindTexture(GL_TEXTURE_2D, texId);
        
		glTexSubImage2D(GL_TEXTURE_2D, 0, px, py, w, h, GL_RGBA, GL_UNSIGNED_BYTE, buf + offset);
        
		return glGetError();
    }
    
    int UpdateTileRaw32(GLuint texId,
                        int px,
                        int py,
                        int w,
                        int h,
                        unsigned char * buf,
                        int offset) {
        return UpdateTileRaw(texId, px, py, w, h, buf, offset);
    }

    int DecodeTile(unsigned char * dataBuf,
                   size_t dataSz,
                   int w,
                   int h,
                   unsigned char * renderBuf,
                   size_t renderSz) {
        
        CGColorSpaceRef colorSpace = CGColorSpaceCreateDeviceRGB();
        
        CGContextRef context =
        CGBitmapContextCreate(renderBuf, 256, 256, 8, 256 * 4, colorSpace, kCGImageAlphaPremultipliedLast);
        
        CGContextTranslateCTM(context, 0, 256);
        CGContextScaleCTM(context, 1.0, -1.0);
        
        CGColorSpaceRelease(colorSpace);
        
        CFDataRef rgbData = CFDataCreate(NULL, dataBuf, dataSz);
        CGDataProviderRef provider = CGDataProviderCreateWithCFData(rgbData);
        
        CGImageRef img =
        CGImageCreateWithJPEGDataProvider(provider, nil, false, kCGRenderingIntentDefault);
        
        CGContextDrawImage(context, CGRectMake(0, 0, w, h), img);
        
        CGContextRelease(context);
        
        return 0;
    }
    
    int UpdateTile(
		GLuint texId, 
		int px,
		int py,
		int w,
		int h,
		unsigned char * buf, 
		int bufSz) {
        
        if (!g_initialized) {
            Initialize();
        }

        CFDataRef rgbData = CFDataCreate(NULL, buf, bufSz);
        CGDataProviderRef provider = CGDataProviderCreateWithCFData(rgbData);
        
        CGImageRef img =
        CGImageCreateWithJPEGDataProvider(provider, nil, false, kCGRenderingIntentDefault);
        
        CGContextDrawImage(g_context, CGRectMake(0, 0, w, h), img);
        
        return UpdateTileRaw(texId, px, py, w, h, g_renderBuf, 0);
	}
}