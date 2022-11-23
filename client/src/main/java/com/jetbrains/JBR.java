/*
 * Copyright 2000-2021 JetBrains s.r.o.
 *
 * Licensed under the Apache License, Version 2.0 (the "License");
 * you may not use this file except in compliance with the License.
 * You may obtain a copy of the License at
 *
 * http://www.apache.org/licenses/LICENSE-2.0
 *
 * Unless required by applicable law or agreed to in writing, software
 * distributed under the License is distributed on an "AS IS" BASIS,
 * WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
 * See the License for the specific language governing permissions and
 * limitations under the License.
 */

package com.jetbrains;

import java.lang.invoke.MethodHandles;
import java.lang.reflect.InvocationTargetException;

/**
 * This class is an entry point into JBR API.
 * JBR API is a collection of services, classes, interfaces, etc.,
 * which require tight interaction with JRE and therefore are implemented inside JBR.
 * <div>JBR API consists of two parts:</div>
 * <ul>
 *     <li>Client side - {@code jetbrains.api} module, mostly containing interfaces</li>
 *     <li>JBR side - actual implementation code inside JBR</li>
 * </ul>
 * Client and JBR side are linked dynamically at runtime and do not have to be of the same version.
 * In some cases (e.g. running on different JRE or old JBR) system will not be able to find
 * implementation for some services, so you'll need a fallback behavior for that case.
 * <h2>Simple usage example:</h2>
 * <blockquote><pre>{@code
 * if (JBR.isSomeServiceSupported()) {
 *     JBR.getSomeService().doSomething();
 * } else {
 *     planB();
 * }
 * }</pre></blockquote>
 * @implNote JBR API is initialized on first access to this class (in static initializer).
 * Actual implementation is linked on demand, when corresponding service is requested by client.
 */
public class JBR {

    private static final ServiceApi api;
    private static final Exception bootstrapException;
    static {
        ServiceApi a = null;
        Exception exception = null;
        try {
            a = (ServiceApi) Class.forName("com.jetbrains.bootstrap.JBRApiBootstrap")
                    .getMethod("bootstrap", MethodHandles.Lookup.class)
                    .invoke(null, MethodHandles.lookup());
        } catch (InvocationTargetException e) {
            Throwable t = e.getCause();
            if (t instanceof Error error) throw error;
            else throw new Error(t);
        } catch (IllegalAccessException | NoSuchMethodException | ClassNotFoundException e) {
            exception = e;
        }
        api = a;
        bootstrapException = exception;
    }

    private JBR() {}

    private static <T> T getService(Class<T> interFace, FallbackSupplier<T> fallback) {
        T service = getService(interFace);
        try {
            return service != null ? service : fallback != null ? fallback.get() : null;
        } catch (Throwable ignore) {
            return null;
        }
    }

    static <T> T getService(Class<T> interFace) {
        return api == null ? null : api.getService(interFace);
    }

    /**
     * @return true when running on JBR which implements JBR API
     */
    public static boolean isAvailable() {
        return api != null;
    }

    /**
     * @return JBR API version in form {@code JBR.MAJOR.MINOR.PATCH}
     * @implNote This is an API version, which comes with client application,
     * it has nothing to do with JRE it runs on.
     */
    public static String getApiVersion() {
        return "17.0.5-b653.14.0.0.7";
    }

    /**
     * Internal API interface, contains most basic methods for communication between client and JBR.
     */
    private interface ServiceApi {

        <T> T getService(Class<T> interFace);
    }

    @FunctionalInterface
    private interface FallbackSupplier<T> {
        T get() throws Throwable;
    }

    // ========================== Generated metadata ==========================

    /**
     * Generated client-side metadata, needed by JBR when linking the implementation.
     */
    private static final class Metadata {
        private static final String[] KNOWN_SERVICES = {"com.jetbrains.ExtendedGlyphCache", "com.jetbrains.DesktopActions", "com.jetbrains.RoundedCornersManager", "com.jetbrains.CustomWindowDecoration", "com.jetbrains.JBRFileDialogService", "com.jetbrains.JBR$ServiceApi", "com.jetbrains.ProjectorUtils"};
        private static final String[] KNOWN_PROXIES = {"com.jetbrains.JBRFileDialog"};
    }

    // ======================= Generated static methods =======================

    private static class ExtendedGlyphCache__Holder {
        private static final ExtendedGlyphCache INSTANCE = getService(ExtendedGlyphCache.class, null);
    }
    /**
     * @return true if current runtime has implementation for all methods in {@link ExtendedGlyphCache}
     * and its dependencies (can fully implement given service).
     * @see #getExtendedGlyphCache()
     */
    public static boolean isExtendedGlyphCacheSupported() {
        return ExtendedGlyphCache__Holder.INSTANCE != null;
    }
    /**
     * @return full implementation of {@link ExtendedGlyphCache} service if any, or {@code null} otherwise
     */
    public static ExtendedGlyphCache getExtendedGlyphCache() {
        return ExtendedGlyphCache__Holder.INSTANCE;
    }

    private static class DesktopActions__Holder {
        private static final DesktopActions INSTANCE = getService(DesktopActions.class, null);
    }
    /**
     * @return true if current runtime has implementation for all methods in {@link DesktopActions}
     * and its dependencies (can fully implement given service).
     * @see #getDesktopActions()
     */
    public static boolean isDesktopActionsSupported() {
        return DesktopActions__Holder.INSTANCE != null;
    }
    /**
     * @return full implementation of {@link DesktopActions} service if any, or {@code null} otherwise
     */
    public static DesktopActions getDesktopActions() {
        return DesktopActions__Holder.INSTANCE;
    }

    private static class RoundedCornersManager__Holder {
        private static final RoundedCornersManager INSTANCE = getService(RoundedCornersManager.class, null);
    }
    /**
     * @return true if current runtime has implementation for all methods in {@link RoundedCornersManager}
     * and its dependencies (can fully implement given service).
     * @see #getRoundedCornersManager()
     */
    public static boolean isRoundedCornersManagerSupported() {
        return RoundedCornersManager__Holder.INSTANCE != null;
    }
    /**
     * This manager allows decorate awt Window with rounded corners.
     * Appearance depends from operating system.
     * @return full implementation of {@link RoundedCornersManager} service if any, or {@code null} otherwise
     */
    public static RoundedCornersManager getRoundedCornersManager() {
        return RoundedCornersManager__Holder.INSTANCE;
    }

    private static class CustomWindowDecoration__Holder {
        private static final CustomWindowDecoration INSTANCE = getService(CustomWindowDecoration.class, null);
    }
    /**
     * @return true if current runtime has implementation for all methods in {@link CustomWindowDecoration}
     * and its dependencies (can fully implement given service).
     * @see #getCustomWindowDecoration()
     */
    public static boolean isCustomWindowDecorationSupported() {
        return CustomWindowDecoration__Holder.INSTANCE != null;
    }
    /**
     * Custom window decoration allows merging of window content with native title bar,
     * which is usually done by treating title bar as part of client area, but with some
     * special behavior like dragging or maximizing on double click.
     * @implNote Behavior is platform-dependent, only macOS and Windows are supported.
     * @return full implementation of {@link CustomWindowDecoration} service if any, or {@code null} otherwise
     */
    public static CustomWindowDecoration getCustomWindowDecoration() {
        return CustomWindowDecoration__Holder.INSTANCE;
    }

    private static class ProjectorUtils__Holder {
        private static final ProjectorUtils INSTANCE = getService(ProjectorUtils.class, null);
    }
    /**
     * @return true if current runtime has implementation for all methods in {@link ProjectorUtils}
     * and its dependencies (can fully implement given service).
     * @see #getProjectorUtils()
     */
    public static boolean isProjectorUtilsSupported() {
        return ProjectorUtils__Holder.INSTANCE != null;
    }
    /**
     * @return full implementation of {@link ProjectorUtils} service if any, or {@code null} otherwise
     */
    public static ProjectorUtils getProjectorUtils() {
        return ProjectorUtils__Holder.INSTANCE;
    }
}
