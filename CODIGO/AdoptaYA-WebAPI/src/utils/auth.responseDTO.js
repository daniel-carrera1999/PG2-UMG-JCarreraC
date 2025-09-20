exports.toLoginResponseDTO = (accessToken, userInstance) => {
    
  const roles = userInstance.rols || [];
  const r = roles.length > 0 ? roles[0] : null;

  return {
    Username: userInstance.username,
    Correo: userInstance.correo,
    Nombre: `${userInstance.nombre} ${userInstance.apellido}`,
    Rol: r.nombre,
    Menu: (r.permisos || []).map(p => ({
        Nombre: p.modulo?.nombre ?? null,
        Icon: p.modulo?.icon ?? null,
        Path: p.modulo?.path ?? null,
        Permisos: {
                Create: !!p.get("create"),
                Read:   !!p.get("read"),
                Update: !!p.get("update"),
                Delete: !!p.get("delete")
        }
    }))
  };
};
