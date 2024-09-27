// src/app/validators/reservas.validators.ts

import { AbstractControl, ValidatorFn } from '@angular/forms';

// Función de validación personalizada para asegurarse de que la diferencia no sea mayor a 24 horas
export function max24HoursValidator(): ValidatorFn {
  return (control: AbstractControl): { [key: string]: any } | null => {
    const horaInicio = control.get('HoraInicio')?.value;
    const horaFin = control.get('HoraFin')?.value;

    if (!horaInicio || !horaFin) {
      return null; // Si no hay valores aún, no hacemos la validación
    }

    const diffMs = new Date(horaFin).getTime() - new Date(horaInicio).getTime();
    const diffHours = diffMs / (1000 * 60 * 60); // Convertir de ms a horas

    if (diffHours > 24) {
      return { max24Hours: true }; // Retornar error si la diferencia es mayor a 24 horas
    }

    return null; // Retornar null si la validación pasa
  };
}
